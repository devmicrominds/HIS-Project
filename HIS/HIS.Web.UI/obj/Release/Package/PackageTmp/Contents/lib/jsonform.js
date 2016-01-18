(function (serverside, global, $, _, JSON) {

    var reArray = /\[([0-9]*)\](?=\[|\.|$)/g;

    var fieldTemplateSettings = {
        evaluate: /<@([\s\S]+?)@>/g,
        interpolate: /<@=([\s\S]+?)@>/g
    };

    var valueTemplateSettings = {
        evaluate: /\{\[([\s\S]+?)\]\}/g,
        interpolate: /\{\{([\s\S]+?)\}\}/g
    };

    var isSet = function (value) {
        return !(_.isUndefined(value) || _.isNull(value));
    };

    var jsonform = { util: {} };

    var escapeHTML = function (string) {
        if (!isSet(string)) {
            return '';
        }
        string = '' + string;
        if (!string) {
            return '';
        }
        return string
          .replace(/&(?!\w+;|#\d+;|#x[\da-f]+;)/gi, '&amp;')
          .replace(/</g, '&lt;')
          .replace(/>/g, '&gt;')
          .replace(/"/g, '&quot;')
          .replace(/'/g, '&#x27;')
          .replace(/\//g, '&#x2F;');
    };

    var dataAttributes = function (attributes) {

        var result = new String();
        _.each(attributes, function (key, value) {
            result += 'data-' + value.replace('_', '-') + '=' + '"' + key + '"' + '  ';
        });

        return result;

    };

    var escapeSelector = function (selector) {
        return selector.replace(/([ \!\"\#\$\%\&\'\(\)\*\+\,\.\/\:\;<\=\>\?\@\[\\\]\^\`\{\|\}\~])/g, '\\$1');
    };

    var initializeTabs = function (tabs) {
        var activate = function (element, container) {
            container
              .find('> .active')
              .removeClass('active');
            element.addClass('active');
        };

        var enableFields = function ($target, targetIndex) {
            // Enable all fields in the targeted tab
            $target.find('input, textarea, select').removeAttr('disabled');

            // Disable all fields in other tabs
            $target.parent()
              .children(':not([data-idx=' + targetIndex + '])')
              .find('input, textarea, select')
              .attr('disabled', 'disabled');
        };

        var optionSelected = function (e) {
            var $option = $("option:selected", $(this)),
              $select = $(this),
              // do not use .attr() as it sometimes unexplicably fails
              targetIdx = $option.get(0).getAttribute('data-idx') || $option.attr('value'),
              $target;

            e.preventDefault();
            if ($option.hasClass('active')) {
                return;
            }

            $target = $(this).parents('.tabbable').eq(0).find('.tab-content [data-idx=' + targetIdx + ']');

            activate($option, $select);
            activate($target, $target.parent());
            enableFields($target, targetIdx);
        };

        var tabClicked = function (e) {
            var $a = $('a', $(this));
            var $content = $(this).parents('.tabbable').first()
              .find('.tab-content').first();
            var targetIdx = $(this).index();
            var $target = $content.find('[data-idx=' + targetIdx + ']');

            e.preventDefault();
            activate($(this), $(this).parent());
            activate($target, $target.parent());
            if ($(this).parent().hasClass('jsonform-alternative')) {
                enableFields($target, targetIdx);
            }
        };

        tabs.each(function () {
            $(this).delegate('select.nav', 'change', optionSelected);
            $(this).find('select.nav').each(function () {
                $(this).val($(this).find('.active').attr('value'));
                // do not use .attr() as it sometimes unexplicably fails
                var targetIdx = $(this).find('option:selected').get(0).getAttribute('data-idx') ||
                  $(this).find('option:selected').attr('value');
                var $target = $(this).parents('.tabbable').eq(0).find('.tab-content [data-idx=' + targetIdx + ']');
                enableFields($target, targetIdx);
            });

            $(this).delegate('ul.nav li', 'click', tabClicked);
            $(this).find('ul.nav li.active').click();
        });
    };

    jsonform.fieldTemplate = function (inner) {
        return '<div class="control-group' +
          '<@= elt.htmlClass ? " " + elt.htmlClass : "" @>' +
          '<@= (node.schemaElement && node.schemaElement.required && (node.schemaElement.type !== "boolean") ? " jsonform-required" : "") @>' +
          '<@= (node.readOnly ? " jsonform-readonly" : "") @>' +
          '<@= (node.disabled ? " jsonform-disabled" : "") @>' +
          '">' +
          '<@ if (node.title && !elt.notitle) { @>' +
            '<label class="control-label" for="<@= node.id @>"><@= node.title @></label>' +
          '<@ } @>' +
          '<div class="controls form-group">' +
            '<@ if (node.prepend || node.append) { @>' +
            '<div class="<@ if (node.prepend) { @>input-prepend<@ } @>' +
              '<@ if (node.append) { @> input-append<@ } @>">' +
              '<@ if (node.prepend) { @>' +
                '<span class="add-on"><@= node.prepend @></span>' +
              '<@ } @>' +
            '<@ } @>' +
            inner +
            '<@ if (node.append) { @>' +
              '<span class="add-on"><@= node.append @></span>' +
            '<@ } @>' +
            '<@ if (node.prepend || node.append) { @>' +
              '</div>' +
            '<@ } @>' +
            '<@ if (node.description) { @>' +
              '<span class="help-inline"><@= node.description @></span>' +
            '<@ } @>' +
            '<span class="help-block" style="display:none;"></span>' +
          '</div></div>';
    };

    var fileDisplayTemplate = '<div class="_jsonform-preview">' +
      '<@ if (value.type=="image") { @>' +
      '<img class="jsonform-preview" id="jsonformpreview-<@= id @>" src="<@= value.url @>" />' +
      '<@ } else { @>' +
      '<a href="<@= value.url @>"><@= value.name @></a> (<@= Math.ceil(value.size/1024) @>kB)' +
      '<@ } @>' +
      '</div>' +
      '<a href="#" class="btn _jsonform-delete"><i class="icon-remove" title="Remove"></i></a> ';

    var inputFieldTemplate = function (type) {
        return {
            'template': '<input type="' + type + '" ' +
              '<@= (fieldHtmlClass ? "class=\'" + fieldHtmlClass + "\' " : "") @>' +
              'name="<@= node.name @>" value="<@= escape(value) @>" id="<@= id @>"' +
              '<@= (node.disabled? " disabled" : "")@>' +
              '<@= (node.readOnly ? " readonly=\'readonly\'" : "") @>' +
              '<@= (node.schemaElement && node.schemaElement.maxLength ? " maxlength=\'" + node.schemaElement.maxLength + "\'" : "") @>' +
              '<@= (node.schemaElement && node.schemaElement.required && (node.schemaElement.type !== "boolean") ? " required=\'required\'" : "") @>' +
              '<@= (node.placeholder? "placeholder=" + \'"\' + escape(node.placeholder) + \'"\' : "")@>' +
              '<@= (node.formElement.dataAttr ? attribute(node.formElement.dataAttr)  : "" )@>' +
              ' />',
            'fieldtemplate': true,
            'inputfield': true
        }
    };

    jsonform.elementTypes = {
        'none': {
            'template': ''
        },
        'root': {
            'template': '<div><@= children @></div>'
        },
        'text': inputFieldTemplate('text'),
        'password': inputFieldTemplate('password'),
        'date': inputFieldTemplate('date'),
        'datetime': inputFieldTemplate('datetime'),
        'datetime-local': inputFieldTemplate('datetime-local'),
        'email': inputFieldTemplate('email'),
        'month': inputFieldTemplate('month'),
        'number': inputFieldTemplate('number'),
        'search': inputFieldTemplate('search'),
        'tel': inputFieldTemplate('tel'),
        'time': inputFieldTemplate('time'),
        'url': inputFieldTemplate('url'),
        'week': inputFieldTemplate('week'),
        'range': {
            'template': '<input type="range" ' +
              '<@= (fieldHtmlClass ? "class=\'" + fieldHtmlClass + "\' " : "") @>' +
              'name="<@= node.name @>" value="<@= escape(value) @>" id="<@= id @>"' +
              '<@= (node.disabled? " disabled" : "")@>' +
              ' min=<@= range.min @>' +
              ' max=<@= range.max @>' +
              ' step=<@= range.step @>' +
              '<@= (node.schemaElement && node.schemaElement.required ? " required=\'required\'" : "") @>' +
              ' />',
            'fieldtemplate': true,
            'inputfield': true,
            'onBeforeRender': function (data, node) {
                data.range = {
                    min: 1,
                    max: 100,
                    step: 1
                };
                if (!node || !node.schemaElement) return;
                if (node.formElement && node.formElement.step) {
                    data.range.step = node.formElement.step;
                }
                if (typeof node.schemaElement.minimum !== 'undefined') {
                    if (node.schemaElement.exclusiveMinimum) {
                        data.range.min = node.schemaElement.minimum + data.range.step;
                    }
                    else {
                        data.range.min = node.schemaElement.minimum;
                    }
                }
                if (typeof node.schemaElement.maximum !== 'undefined') {
                    if (node.schemaElement.exclusiveMaximum) {
                        data.range.max = node.schemaElement.maximum + data.range.step;
                    }
                    else {
                        data.range.max = node.schemaElement.maximum;
                    }
                }
            }
        },
        'color': {
            'template': '<input type="text" ' +
              '<@= (fieldHtmlClass ? "class=\'" + fieldHtmlClass + "\' " : "") @>' +
              'name="<@= node.name @>" value="<@= escape(value) @>" id="<@= id @>"' +
              '<@= (node.disabled? " disabled" : "")@>' +
              '<@= (node.schemaElement && node.schemaElement.required ? " required=\'required\'" : "") @>' +
              ' />',
            'fieldtemplate': true,
            'inputfield': true,
            'onInsert': function (evt, node) {
                $(node.el).find('#' + escapeSelector(node.id)).spectrum({
                    preferredFormat: "hex",
                    showInput: true
                });
            }
        },
        'textarea': {
            'template': '<textarea id="<@= id @>" name="<@= node.name @>" ' +
              'style="height:<@= elt.height || "150px" @>;width:<@= elt.width || "100%" @>;"' +
              '<@= (node.disabled? " disabled" : "")@>' +
              '<@= (node.readOnly ? " readonly=\'readonly\'" : "") @>' +
              '<@= (node.schemaElement && node.schemaElement.maxLength ? " maxlength=\'" + node.schemaElement.maxLength + "\'" : "") @>' +
              '<@= (node.schemaElement && node.schemaElement.required ? " required=\'required\'" : "") @>' +
              '<@= (node.placeholder? "placeholder=" + \'"\' + escape(node.placeholder) + \'"\' : "")@>' +
              '><@= value @></textarea>',
            'fieldtemplate': true,
            'inputfield': true
        },
        'wysihtml5': {
            'template': '<textarea id="<@= id @>" name="<@= node.name @>" style="height:<@= elt.height || "300px" @>;width:<@= elt.width || "100%" @>;"' +
              '<@= (node.disabled? " disabled" : "")@>' +
              '<@= (node.readOnly ? " readonly=\'readonly\'" : "") @>' +
              '<@= (node.schemaElement && node.schemaElement.maxLength ? " maxlength=\'" + node.schemaElement.maxLength + "\'" : "") @>' +
              '<@= (node.schemaElement && node.schemaElement.required ? " required=\'required\'" : "") @>' +
              '<@= (node.placeholder? "placeholder=" + \'"\' + escape(node.placeholder) + \'"\' : "")@>' +
              '><@= value @></textarea>',
            'fieldtemplate': true,
            'inputfield': true,
            'onInsert': function (evt, node) {
                var setup = function () {
                    //protect from double init
                    if ($(node.el).data("wysihtml5")) return;
                    $(node.el).data("wysihtml5_loaded", true);

                    $(node.el).find('#' + escapeSelector(node.id)).wysihtml5({
                        "html": true,
                        "link": true,
                        "font-styles": true,
                        "image": true,
                        "events": {
                            "load": function () {
                                // In chrome, if an element is required and hidden, it leads to
                                // the error 'An invalid form control with name='' is not focusable'
                                // See http://stackoverflow.com/questions/7168645/invalid-form-control-only-in-google-chrome
                                $(this.textareaElement).removeAttr('required');
                            }
                        }
                    });
                };

                // Is there a setup hook?
                if (window.jsonform_wysihtml5_setup) {
                    window.jsonform_wysihtml5_setup(setup);
                    return;
                }

                // Wait until wysihtml5 is loaded
                var itv = window.setInterval(function () {
                    if (window.wysihtml5) {
                        window.clearInterval(itv);
                        setup();
                    }
                }, 1000);
            }
        },
        'checkbox': {
            'template': '<label class="checkbox"><input type="checkbox" id="<@= id @>" ' +
              'name="<@= node.name @>" value="1" <@ if (value) {@>checked<@ } @>' +
              '<@= (node.disabled? " disabled" : "")@>' +
              '<@= (node.schemaElement && node.schemaElement.required && (node.schemaElement.type !== "boolean") ? " required=\'required\'" : "") @>' +
              ' /><span><@= node.inlinetitle || "" @></span>' +
              '</label>',
            'fieldtemplate': true,
            'inputfield': true,
            'getElement': function (el) {
                return $(el).parent().get(0);
            }
        },
        'file': {
            'template': '<input class="input-file" id="<@= id @>" name="<@= node.name @>" type="file" ' +
              '<@= (node.schemaElement && node.schemaElement.required ? " required=\'required\'" : "") @>' +
              '/>',
            'fieldtemplate': true,
            'inputfield': true
        },
        'select': {
            'template': '<select name="<@= node.name @>" id="<@= id @>"' +
              '<@= (fieldHtmlClass ? " class=\'" + fieldHtmlClass + "\'" : "") @>' +
              '<@= (node.disabled? " disabled" : "")@>' +
              '<@= (node.schemaElement && node.schemaElement.required ? " required=\'required\'" : "") @>' +
              '<@= (node.formElement.dataAttr ? attribute(node.formElement.dataAttr)  : "" )@>' +
              ' >' +
              '<@ _.each(node.formElement.items, function(key, val) { if(key instanceof Object) { if (value === key.value) { @> <option selected value="<@= key.value @>"><@= key.title @></option> <@ } else { @> <option value="<@= key.value @>"><@= key.title @></option> <@ }} else { if (value === key) { @> <option selected value="<@= key @>"><@= key @></option> <@ } else { @><option value="<@= key @>"><@= key @></option> <@ }}}); @> ' +
              '</select>',
            'fieldtemplate': true,
            'inputfield': true
        },
        'imageselect': {
            'template': '<div>' +
              '<input type="hidden" name="<@= node.name @>" id="<@= node.id @>" value="<@= value @>" />' +
              '<div class="dropdown">' +
              '<a class="btn<@ if (buttonClass && node.value) { @> <@= buttonClass @><@ } @>" data-toggle="dropdown" href="#"<@ if (node.value) { @> style="max-width:<@= width @>px;max-height:<@= height @>px"<@ } @>>' +
                '<@ if (node.value) { @><img src="<@ if (!node.value.match(/^https?:/)) { @><@= prefix @><@ } @><@= node.value @><@= suffix @>" alt="" /><@ } else { @><@= buttonTitle @><@ } @>' +
              '</a>' +
              '<div class="dropdown-menu navbar" id="<@= node.id @>_dropdown">' +
                '<div>' +
                '<@ _.each(node.options, function(key, idx) { if ((idx > 0) && ((idx % columns) === 0)) { @></div><div><@ } @><a class="btn<@ if (buttonClass) { @> <@= buttonClass @><@ } @>" style="max-width:<@= width @>px;max-height:<@= height @>px"><@ if (key instanceof Object) { @><img src="<@ if (!key.value.match(/^https?:/)) { @><@= prefix @><@ } @><@= key.value @><@= suffix @>" alt="<@= key.title @>" /></a><@ } else { @><img src="<@ if (!key.match(/^https?:/)) { @><@= prefix @><@ } @><@= key @><@= suffix @>" alt="" /><@ } @></a> <@ }); @>' +
                '</div>' +
                '<div class="pagination-right"><a class="btn">Reset</a></div>' +
              '</div>' +
              '</div>' +
              '</div>',
            'fieldtemplate': true,
            'inputfield': true,
            'onBeforeRender': function (data, node) {
                var elt = node.formElement || {};
                var nbRows = null;
                var maxColumns = elt.imageSelectorColumns || 5;
                data.buttonTitle = elt.imageSelectorTitle || 'Select...';
                data.prefix = elt.imagePrefix || '';
                data.suffix = elt.imageSuffix || '';
                data.width = elt.imageWidth || 32;
                data.height = elt.imageHeight || 32;
                data.buttonClass = elt.imageButtonClass || false;
                if (node.options.length > maxColumns) {
                    nbRows = Math.ceil(node.options.length / maxColumns);
                    data.columns = Math.ceil(node.options.length / nbRows);
                }
                else {
                    data.columns = maxColumns;
                }
            },
            'getElement': function (el) {
                return $(el).parent().get(0);
            },
            'onInsert': function (evt, node) {
                $(node.el).on('click', '.dropdown-menu a', function (evt) {
                    evt.preventDefault();
                    evt.stopPropagation();
                    var img = (evt.target.nodeName.toLowerCase() === 'img') ?
                      $(evt.target) :
                      $(evt.target).find('img');
                    var value = img.attr('src');
                    var elt = node.formElement || {};
                    var prefix = elt.imagePrefix || '';
                    var suffix = elt.imageSuffix || '';
                    var width = elt.imageWidth || 32;
                    var height = elt.imageHeight || 32;
                    if (value) {
                        if (value.indexOf(prefix) === 0) {
                            value = value.substring(prefix.length);
                        }
                        value = value.substring(0, value.length - suffix.length);
                        $(node.el).find('input').attr('value', value);
                        $(node.el).find('a[data-toggle="dropdown"]')
                          .addClass(elt.imageButtonClass)
                          .attr('style', 'max-width:' + width + 'px;max-height:' + height + 'px')
                          .html('<img src="' + (!value.match(/^https?:/) ? prefix : '') + value + suffix + '" alt="" />');
                    }
                    else {
                        $(node.el).find('input').attr('value', '');
                        $(node.el).find('a[data-toggle="dropdown"]')
                          .removeClass(elt.imageButtonClass)
                          .removeAttr('style')
                          .html(elt.imageSelectorTitle || 'Select...');
                    }
                });
            }
        },
        'radios': {
            'template': '<div id="<@= node.id @>"><@ _.each(node.options, function(key, val) { @><label class="radio"><input type="radio" <@ if (((key instanceof Object) && (value === key.value)) || (value === key)) { @> checked="checked" <@ } @> name="<@= node.name @>" value="<@= (key instanceof Object ? key.value : key) @>"' +
              '<@= (node.disabled? " disabled" : "")@>' +
              '<@= (node.schemaElement && node.schemaElement.required ? " required=\'required\'" : "") @>' +
              '/><span><@= (key instanceof Object ? key.title : key) @></span></label> <@ }); @></div>',
            'fieldtemplate': true,
            'inputfield': true
        },
        'radiobuttons': {
            'template': '<div id="<@= node.id @>">' +
              '<@ _.each(node.options, function(key, val) { @>' +
                '<label class="radio btn">' +
                '<input type="radio" style="position:absolute;left:-9999px;" ' +
                '<@ if (((key instanceof Object) && (value === key.value)) || (value === key)) { @> checked="checked" <@ } @> name="<@= node.name @>" value="<@= (key instanceof Object ? key.value : key) @>" />' +
                '<span><@= (key instanceof Object ? key.title : key) @></span></label> ' +
                '<@ }); @>' +
              '</div>',
            'fieldtempate': true,
            'inputfield': true,
            'onInsert': function (evt, node) {
                var activeClass = 'active';
                var elt = node.formElement || {};
                if (elt.activeClass) {
                    activeClass += ' ' + elt.activeClass;
                }
                $(node.el).find('label').on('click', function () {
                    $(this).parent().find('label').removeClass(activeClass);
                    $(this).addClass(activeClass);
                });
            }
        },
        'checkboxes': {
            'template': '<div><@= choiceshtml @></div>',
            'fieldtemplate': true,
            'inputfield': true,
            'onBeforeRender': function (data, node) {
                // Build up choices from the enumeration list
                var choices = null;
                var choiceshtml = null;
                var template = '<label class="checkbox">' +
                  '<input type="checkbox" <@ if (value) { @> checked="checked" <@ } @> name="<@= name @>" value="1"' +
                  '<@= (node.disabled? " disabled" : "")@>' +
                  '/><span><@= title @></span></label>';
                if (!node || !node.schemaElement || !node.schemaElement.items) return;
                choices = node.schemaElement.items['enum'] ||
                  node.schemaElement.items[0]['enum'];
                if (!choices) return;

                choiceshtml = '';
                _.each(choices, function (choice, idx) {
                    choiceshtml += _.template(template, {
                        name: node.key + '[' + idx + ']',
                        value: _.include(node.value, choice),
                        title: node.formElement.titleMap ? node.formElement.titleMap[choice] : choice,
                        node: node
                    }, fieldTemplateSettings);
                });

                data.choiceshtml = choiceshtml;
            }
        },
        'array': {
            'template': '<div id="<@= id @>"><ul class="_jsonform-array-ul" style="list-style-type:none;"><@= children @></ul>' +
              '<span class="_jsonform-array-buttons">' +
                '<a href="#" class="btn btn-primary btn-sm _jsonform-array-addmore"><i class="glyphicon glyphicon-plus" title="Add new"></i></a> ' +
                '<a href="#" class="btn btn-primary btn-sm _jsonform-array-deletelast"><i class="glyphicon glyphicon-minus" title="Delete last"></i></a>' +
              '</span>' +
              '</div>',
            'fieldtemplate': true,
            'array': true,
            'childTemplate': function (inner) {
                if ($('').sortable) {
                    // Insert a "draggable" icon
                    // floating to the left of the main element
                    return '<li data-idx="<@= node.childPos @>">' +
                      '<span class="draggable line"><i class="icon-list" title="Move item"></i></span>' +
                      inner +
                      '</li>';
                }
                else {
                    return '<li data-idx="<@= node.childPos @>">' +
                      inner +
                      '</li>';
                }
            },
            'onInsert': function (evt, node) {
                var $nodeid = $(node.el).find('#' + escapeSelector(node.id));
                var boundaries = node.getArrayBoundaries();

                // Switch two nodes in an array
                var moveNodeTo = function (fromIdx, toIdx) {
                    // Note "switchValuesWith" extracts values from the DOM since field
                    // values are not synchronized with the tree data structure, so calls
                    // to render are needed at each step to force values down to the DOM
                    // before next move.
                    // TODO: synchronize field values and data structure completely and
                    // call render only once to improve efficiency.
                    if (fromIdx === toIdx) return;
                    var incr = (fromIdx < toIdx) ? 1 : -1;
                    var i = 0;
                    var parentEl = $('> ul', $nodeid);
                    for (i = fromIdx; i !== toIdx; i += incr) {
                        node.children[i].switchValuesWith(node.children[i + incr]);
                        node.children[i].render(parentEl.get(0));
                        node.children[i + incr].render(parentEl.get(0));
                    }

                    // No simple way to prevent DOM reordering with jQuery UI Sortable,
                    // so we're going to need to move sorted DOM elements back to their
                    // origin position in the DOM ourselves (we switched values but not
                    // DOM elements)
                    var fromEl = $(node.children[fromIdx].el);
                    var toEl = $(node.children[toIdx].el);
                    fromEl.detach();
                    toEl.detach();
                    if (fromIdx < toIdx) {
                        if (fromIdx === 0) parentEl.prepend(fromEl);
                        else $(node.children[fromIdx - 1].el).after(fromEl);
                        $(node.children[toIdx - 1].el).after(toEl);
                    }
                    else {
                        if (toIdx === 0) parentEl.prepend(toEl);
                        else $(node.children[toIdx - 1].el).after(toEl);
                        $(node.children[fromIdx - 1].el).after(fromEl);
                    }
                };

                $('> span > a._jsonform-array-addmore', $nodeid).click(function (evt) {
                    evt.preventDefault();
                    evt.stopPropagation();
                    var idx = node.children.length;
                    if (boundaries.maxItems >= 0) {
                        if (node.children.length > boundaries.maxItems - 2) {
                            $nodeid.find('> span > a._jsonform-array-addmore')
                              .addClass('disabled');
                        }
                        if (node.children.length > boundaries.maxItems - 1) {
                            return false;
                        }
                    }
                    node.insertArrayItem(idx, $('> ul', $nodeid).get(0));
                    if ((boundaries.minItems <= 0) ||
                        ((boundaries.minItems > 0) &&
                          (node.children.length > boundaries.minItems - 1))) {
                        $nodeid.find('> span > a._jsonform-array-deletelast')
                          .removeClass('disabled');
                    }
                });

                //Simulate Users click to setup the form with its minItems
                var curItems = $('> ul > li', $nodeid).length;
                if ((boundaries.minItems > 0) &&
                    (curItems < boundaries.minItems)) {
                    for (var i = 0; i < (boundaries.minItems - 1) && ($nodeid.find('> ul > li').length < boundaries.minItems) ; i++) {
                        //console.log('Calling click: ',$nodeid);
                        //$('> span > a._jsonform-array-addmore', $nodeid).click();
                        node.insertArrayItem(curItems, $nodeid.find('> ul').get(0));
                    }
                }
                if ((boundaries.minItems > 0) &&
                    (node.children.length <= boundaries.minItems)) {
                    $nodeid.find('> span > a._jsonform-array-deletelast')
                      .addClass('disabled');
                }

                $('> span > a._jsonform-array-deletelast', $nodeid).click(function (evt) {
                   
                    var idx = node.children.length - 1;
                    evt.preventDefault();
                    evt.stopPropagation();
                    if (boundaries.minItems > 0) {
                        if (node.children.length < boundaries.minItems + 2) {
                            $nodeid.find('> span > a._jsonform-array-deletelast')
                              .addClass('disabled');
                        }
                        if (node.children.length <= boundaries.minItems) {
                            return false;
                        }
                    }
                    else if (node.children.length === 1) {
                        $nodeid.find('> span > a._jsonform-array-deletelast')
                          .addClass('disabled');
                    }
                    node.deleteArrayItem(idx);
                    if ((boundaries.maxItems >= 0) && (idx <= boundaries.maxItems - 1)) {
                        $nodeid.find('> span > a._jsonform-array-addmore')
                          .removeClass('disabled');
                    }
                });

                if ($(node.el).sortable) {
                    $('> ul', $nodeid).sortable();
                    $('> ul', $nodeid).bind('sortstop', function (event, ui) {
                        var idx = $(ui.item).data('idx');
                        var newIdx = $(ui.item).index();
                        moveNodeTo(idx, newIdx);
                    });
                }
            }
        },
        'tabarray': {
            'template': '<div id="<@= id @>"><div class="tabbable tabs-left">' +
              '<ul class="nav nav-tabs">' +
                '<@= tabs @>' +
              '</ul>' +
              '<div class="tab-content">' +
                '<@= children @>' +
              '</div>' +
              '</div>' +
              '<a href="#" class="btn _jsonform-array-addmore"><i class="icon-plus-sign" title="Add new"></i></a> ' +
              '<a href="#" class="btn _jsonform-array-deleteitem"><i class="icon-minus-sign" title="Delete item"></i></a></div>',
            'fieldtemplate': true,
            'array': true,
            'childTemplate': function (inner) {
                return '<div data-idx="<@= node.childPos @>" class="tab-pane">' +
                  inner +
                  '</div>';
            },
            'onBeforeRender': function (data, node) {
                // Generate the initial 'tabs' from the children
                var tabs = '';
                _.each(node.children, function (child, idx) {
                    var title = child.legend ||
                      child.title ||
                      ('Item ' + (idx + 1));
                    tabs += '<li data-idx="' + idx + '"' +
                      ((idx === 0) ? ' class="active"' : '') +
                      '><a class="draggable tab" data-toggle="tab">' +
                      escapeHTML(title) +
                      '</a></li>';
                });
                data.tabs = tabs;
            },
            'onInsert': function (evt, node) {
                var $nodeid = $(node.el).find('#' + escapeSelector(node.id));
                var boundaries = node.getArrayBoundaries();

                var moveNodeTo = function (fromIdx, toIdx) {
                    // Note "switchValuesWith" extracts values from the DOM since field
                    // values are not synchronized with the tree data structure, so calls
                    // to render are needed at each step to force values down to the DOM
                    // before next move.
                    // TODO: synchronize field values and data structure completely and
                    // call render only once to improve efficiency.
                    if (fromIdx === toIdx) return;
                    var incr = (fromIdx < toIdx) ? 1 : -1;
                    var i = 0;
                    var tabEl = $('> .tabbable > .tab-content', $nodeid).get(0);
                    for (i = fromIdx; i !== toIdx; i += incr) {
                        node.children[i].switchValuesWith(node.children[i + incr]);
                        node.children[i].render(tabEl);
                        node.children[i + incr].render(tabEl);
                    }
                };


                // Refreshes the list of tabs
                var updateTabs = function (selIdx) {
                    var tabs = '';
                    var activateFirstTab = false;
                    if (selIdx === undefined) {
                        selIdx = $('> .tabbable > .nav-tabs .active', $nodeid).data('idx');
                        if (selIdx) {
                            selIdx = parseInt(selIdx, 10);
                        }
                        else {
                            activateFirstTab = true;
                            selIdx = 0;
                        }
                    }
                    if (selIdx >= node.children.length) {
                        selIdx = node.children.length - 1;
                    }
                    _.each(node.children, function (child, idx) {
                        var title = child.legend ||
                          child.title ||
                          ('Item ' + (idx + 1));
                        tabs += '<li data-idx="' + idx + '">' +
                          '<a class="draggable tab" data-toggle="tab">' +
                          escapeHTML(title) +
                          '</a></li>';
                    });
                    $('> .tabbable > .nav-tabs', $nodeid).html(tabs);
                    if (activateFirstTab) {
                        $('> .tabbable > .nav-tabs [data-idx="0"]', $nodeid).addClass('active');
                    }
                    $('> .tabbable > .nav-tabs [data-toggle="tab"]', $nodeid).eq(selIdx).click();
                };

                $('> a._jsonform-array-deleteitem', $nodeid).click(function (evt) {
                    var idx = $('> .tabbable > .nav-tabs .active', $nodeid).data('idx');
                    evt.preventDefault();
                    evt.stopPropagation();
                    if (boundaries.minItems > 0) {
                        if (node.children.length < boundaries.minItems + 1) {
                            $nodeid.find('> a._jsonform-array-deleteitem')
                              .addClass('disabled');
                        }
                        if (node.children.length <= boundaries.minItems) return false;
                    }
                    node.deleteArrayItem(idx);
                    updateTabs();
                    if ((node.children.length < boundaries.minItems + 1) ||
                        (node.children.length === 0)) {
                        $nodeid.find('> a._jsonform-array-deleteitem').addClass('disabled');
                    }
                    if ((boundaries.maxItems >= 0) &&
                        (node.children.length <= boundaries.maxItems)) {
                        $nodeid.find('> a._jsonform-array-addmore').removeClass('disabled');
                    }
                });

                $('> a._jsonform-array-addmore', $nodeid).click(function (evt) {
                    var idx = node.children.length;
                    if (boundaries.maxItems >= 0) {
                        if (node.children.length > boundaries.maxItems - 2) {
                            $('> a._jsonform-array-addmore', $nodeid).addClass("disabled");
                        }
                        if (node.children.length > boundaries.maxItems - 1) {
                            return false;
                        }
                    }
                    evt.preventDefault();
                    evt.stopPropagation();
                    node.insertArrayItem(idx,
                      $nodeid.find('> .tabbable > .tab-content').get(0));
                    updateTabs(idx);
                    if ((boundaries.minItems <= 0) ||
                        ((boundaries.minItems > 0) && (idx > boundaries.minItems - 1))) {
                        $nodeid.find('> a._jsonform-array-deleteitem').removeClass('disabled');
                    }
                });

                $(node.el).on('legendUpdated', function (evt) {
                    updateTabs();
                    evt.preventDefault();
                    evt.stopPropagation();
                });

                if ($(node.el).sortable) {
                    $('> .tabbable > .nav-tabs', $nodeid).sortable({
                        containment: node.el,
                        tolerance: 'pointer'
                    });
                    $('> .tabbable > .nav-tabs', $nodeid).bind('sortstop', function (event, ui) {
                        var idx = $(ui.item).data('idx');
                        var newIdx = $(ui.item).index();
                        moveNodeTo(idx, newIdx);
                        updateTabs(newIdx);
                    });
                }

                // Simulate User's click to setup the form with its minItems
                if (boundaries.minItems >= 0) {
                    for (var i = 0; i < (boundaries.minItems - 1) ; i++) {
                        $nodeid.find('> a._jsonform-array-addmore').click();
                    }
                    $nodeid.find('> a._jsonform-array-deleteitem').addClass('disabled');
                    updateTabs();
                }

                if ((boundaries.maxItems >= 0) &&
                    (node.children.length >= boundaries.maxItems)) {
                    $nodeid.find('> a._jsonform-array-addmore').addClass('disabled');
                }
                if ((boundaries.minItems >= 0) &&
                    (node.children.length <= boundaries.minItems)) {
                    $nodeid.find('> a._jsonform-array-deleteitem').addClass('disabled');
                }
            }
        },
        'help': {
            'template': '<span class="help-block" style="padding-top:5px"><@= elt.helpvalue @></span>',
            'fieldtemplate': true
        },
        'msg': {
            'template': '<@= elt.msg @>'
        },
        'fieldset': {
            'template': '<fieldset class="control-group  <@ if (elt.expandable) { @>expandable<@ } @> <@= elt.htmlClass?elt.htmlClass:"" @>" ' +
              '<@ if (id) { @> id="<@= id @>"<@ } @>' +
              '>' +
              '<@ if (node.title || node.legend) { @><legend><@= node.title || node.legend @></legend><@ } @>' +
              '<@ if (elt.expandable) { @><div class="control-group"><@ } @>' +
              '<@= children @>' +
              '<@ if (elt.expandable) { @></div><@ } @>' +
              '</fieldset>'
        },
        'advancedfieldset': {
            'template': '<fieldset' +
              '<@ if (id) { @> id="<@= id @>"<@ } @>' +
              ' class="expandable <@= elt.htmlClass?elt.htmlClass:"" @>">' +
              '<legend>Advanced options</legend>' +
              '<div class="control-group">' +
              '<@= children @>' +
              '</div>' +
              '</fieldset>'
        },
        'authfieldset': {
            'template': '<fieldset' +
              '<@ if (id) { @> id="<@= id @>"<@ } @>' +
              ' class="expandable <@= elt.htmlClass?elt.htmlClass:"" @>">' +
              '<legend>Authentication settings</legend>' +
              '<div class="control-group">' +
              '<@= children @>' +
              '</div>' +
              '</fieldset>'
        },
        'submit': {
            'template': '<input type="submit" <@ if (id) { @> id="<@= id @>" <@ } @> class="btn btn-primary <@= elt.htmlClass?elt.htmlClass:"" @>" value="<@= value || node.title @>"<@= (node.disabled? " disabled" : "")@>/>'
        },
        'button': {
            'template': ' <button type="button" <@ if (id) { @> id="<@= id @>" <@ } @> class="btn <@= elt.htmlClass?elt.htmlClass:"" @>"><@= node.title @></button> '
        },
        'actions': {
            'template': '<div class="form-actions <@= elt.htmlClass?elt.htmlClass:"" @>"><@= children @></div>'
        },
        'hidden': {
            'template': '<input type="hidden" id="<@= id @>" name="<@= node.name @>" value="<@= escape(value) @>" />',
            'inputfield': true
        },
        'selectfieldset': {
            'template': '<fieldset class="tab-container <@= elt.htmlClass?elt.htmlClass:"" @>">' +
              '<@ if (node.legend) { @><legend><@= node.legend @></legend><@ } @>' +
              '<@ if (node.formElement.key) { @><input type="hidden" id="<@= node.id @>" name="<@= node.name @>" value="<@= escape(value) @>" /><@ } else { @>' +
                '<a id="<@= node.id @>"></a><@ } @>' +
              '<div class="tabbable">' +
                '<div class="control-group<@= node.formElement.hideMenu ? " hide" : "" @>">' +
                  '<@ if (node.title && !elt.notitle) { @><label class="control-label" for="<@= node.id @>"><@= node.title @></label><@ } @>' +
                  '<div class="controls"><@= tabs @></div>' +
                '</div>' +
                '<div class="tab-content">' +
                  '<@= children @>' +
                '</div>' +
              '</div>' +
              '</fieldset>',
            'inputfield': true,
            'getElement': function (el) {
                return $(el).parent().get(0);
            },
            'childTemplate': function (inner) {
                return '<div data-idx="<@= node.childPos @>" class="tab-pane' +
                  '<@ if (node.active) { @> active<@ } @>">' +
                  inner +
                  '</div>';
            },
            'onBeforeRender': function (data, node) {
                var children = null;
                var choices = [];
                if (node.schemaElement) {
                    choices = node.schemaElement['enum'] || [];
                }
                if (node.options) {
                    children = _.map(node.options, function (option, idx) {
                        var child = node.children[idx];
                        if (option instanceof Object) {
                            option = _.extend({ node: child }, option);
                            option.title = option.title ||
                              child.legend ||
                              child.title ||
                              ('Option ' + (child.childPos + 1));
                            option.value = isSet(option.value) ? option.value :
                              isSet(choices[idx]) ? choices[idx] : idx;
                            return option;
                        }
                        else {
                            return {
                                title: option,
                                value: isSet(choices[child.childPos]) ?
                                  choices[child.childPos] :
                                  child.childPos,
                                node: child
                            };
                        }
                    });
                }
                else {
                    children = _.map(node.children, function (child, idx) {
                        return {
                            title: child.legend || child.title || ('Option ' + (child.childPos + 1)),
                            value: choices[child.childPos] || child.childPos,
                            node: child
                        };
                    });
                }

                var activeChild = null;
                if (data.value) {
                    activeChild = _.find(children, function (child) {
                        return (child.value === node.value);
                    });
                }
                if (!activeChild) {
                    activeChild = _.find(children, function (child) {
                        return child.node.hasNonDefaultValue();
                    });
                }
                if (!activeChild) {
                    activeChild = children[0];
                }
                activeChild.node.active = true;
                data.value = activeChild.value;

                var elt = node.formElement;
                var tabs = '<select class="nav"' +
                  (node.disabled ? ' disabled' : '') +
                  '>';
                _.each(children, function (child, idx) {
                    tabs += '<option data-idx="' + idx + '" value="' + child.value + '"' +
                      (child.node.active ? ' class="active"' : '') +
                      '>' +
                      escapeHTML(child.title) +
                      '</option>';
                });
                tabs += '</select>';

                data.tabs = tabs;
                return data;
            },
            'onInsert': function (evt, node) {
                $(node.el).find('select.nav').first().on('change', function (evt) {
                    var $option = $(this).find('option:selected');
                    $(node.el).find('input[type="hidden"]').first().val($option.attr('value'));
                });
            }
        },
        'optionfieldset': {
            'template': '<div' +
              '<@ if (node.id) { @> id="<@= node.id @>"<@ } @>' +
              '>' +
              '<@= children @>' +
              '</div>'
        },
        'section': {
            'template': '<div' +
              '<@ if (node.id) { @> id="<@= node.id @>"<@ } @>' +
              '><@= children @></div>'
        },
        'filter': {
            'template': '<div id="<@= id @>" class="col-lg-12">' +
              '<div class="well">' +
              '<ul>' +
              '<@= children @>' +
              '</ul>' +
              '</div>' +
               '<div class="_jsonform-array-buttons btn-group pull-right panel">' +
                    '<button type="button" class="_addCommand btn btn-sm btn-primary"><i class="glyphicon glyphicon-plus" title="Add new"></i></button> ' +
                    '<button type="button" class="_removeCommand btn btn-sm btn-danger"><i class="glyphicon glyphicon-minus" title="Delete last"></i></button>' +
                    '</div>' +
               '</div>'
              ,
            'fieldtemplate':true,
            'array': true,
            'childTemplate': function (inner)
             {
                
                return '<li data-idx="<@= node.childPos @>">' +
                    '<div class="row">' +
                    '<div class="col-lg-4">' +
                    '<select  id="selection_<@= node.childPos  @>" name="<@= node.name @>" id="<@= id @>" class="selectpicker field col-lg-4" data-style="btn-primary" data-width="auto"' +
                    '<@= (node.disabled? " disabled" : "")@>' +
                    '<@= (node.schemaElement && node.schemaElement.required ? " required=\'required\'" : "") @>' +
                    '<@= (node.formElement.dataAttr ? attribute(node.formElement.dataAttr)  : "" )@>' +
                    '>  '+
                    '<@  _.each(node.formElement.properties, function(data,index) { @>' +
                    '<@ if(!data.disabled) { @>   '+
                    '   <option value="<@= data.key @>"><@= data.title @></option> <@ }' +
                    '}); @> ' +
                    '</select>' +
                    '</div>' +
                    '<div class="col-lg-4 parameter">' +
                    '</div>' +
                    '<div class="col-lg-4">' +
                    '</div>' +
                    '</div>' +
                    '<br/>' +
                    inner +
                    '</li>';

            },
            'onInsert': function (evt, node) {
               
                var $nodeid = $(node.el).find('#' + escapeSelector(node.id));
                var boundaries = node.getArrayBoundaries();
                var filterresult = $('div.parameter', $nodeid.selector);
                var field = $('select.field', $nodeid.selector);
                var addCommand = $('button._addCommand', $nodeid.selector);
                var removeCommand = $('button._removeCommand', $nodeid.selector);

                $(field).on('change', function () {

                    console.log('aABB');
                });

                $(addCommand).on('click', function (evt)   {   
                    evt.preventDefault();
                    evt.stopPropagation();
                    var idx = node.children.length;
                    
                    if (idx != 0)
                    {
                        var prvidx = idx - 1;
                        var prvVal = $('#selection_' + prvidx).val();                         
                        _.find(node.formElement.items[0].properties, function (data,index) {
                            if (data.key == prvVal) {
                                data.disabled = true;
                                return true;
                            }
                        });
                        
                    }

                    node.insertArrayItem(idx, $('> div > ul', $nodeid.selector).get(0));

                    $('#selection_' + idx).on('change', function ()
                    {
                        console.log('AAA')
                    });

                    $('.selectpicker').selectpicker();
                });

                $(removeCommand).on('click', function (evt) {
                    
                    evt.preventDefault();
                    evt.stopPropagation();
                    var idx = node.children.length - 1;
                    node.deleteArrayItem(idx);
                   
                }); 

            }
        }

    };



    jsonform.util.getObjKey = function (obj, key, ignoreArrays) {
        var innerobj = obj;
        var keyparts = key.split(".");
        var subkey = null;
        var arrayMatch = null;
        var prop = null;

        for (var i = 0; i < keyparts.length; i++) {
            if ((innerobj === null) || (typeof innerobj !== "object")) return null;
            subkey = keyparts[i];
            prop = subkey.replace(reArray, '');
            reArray.lastIndex = 0;
            arrayMatch = reArray.exec(subkey);
            if (arrayMatch) {
                while (true) {
                    if (!_.isArray(innerobj[prop])) return null;
                    innerobj = innerobj[prop][parseInt(arrayMatch[1], 10)];
                    arrayMatch = reArray.exec(subkey);
                    if (!arrayMatch) break;
                }
            }
            else if (ignoreArrays &&
                !innerobj[prop] &&
                _.isArray(innerobj) &&
                innerobj[0]) {
                innerobj = innerobj[0][prop];
            }
            else {
                innerobj = innerobj[prop];
            }
        }

        if (ignoreArrays && _.isArray(innerobj) && innerobj[0]) {
            return innerobj[0];
        }
        else {
            return innerobj;
        }
    };



    jsonform.util.setObjKey = function (obj, key, value) {
        var innerobj = obj;
        var keyparts = key.split(".");
        var subkey = null;
        var arrayMatch = null;
        var prop = null;

        for (var i = 0; i < keyparts.length - 1; i++) {
            subkey = keyparts[i];
            prop = subkey.replace(reArray, '');
            reArray.lastIndex = 0;
            arrayMatch = reArray.exec(subkey);
            if (arrayMatch) {
                // Subkey is part of an array
                while (true) {
                    if (!_.isArray(innerobj[prop])) {
                        innerobj[prop] = [];
                    }
                    innerobj = innerobj[prop];
                    prop = parseInt(arrayMatch[1], 10);
                    arrayMatch = reArray.exec(subkey);
                    if (!arrayMatch) break;
                }
                if ((typeof innerobj[prop] !== 'object') ||
                  (innerobj[prop] === null)) {
                    innerobj[prop] = {};
                }
                innerobj = innerobj[prop];
            }
            else {
                // "Normal" subkey
                if ((typeof innerobj[prop] !== 'object') ||
                  (innerobj[prop] === null)) {
                    innerobj[prop] = {};
                }
                innerobj = innerobj[prop];
            }
        }

        // Set the final value
        subkey = keyparts[keyparts.length - 1];
        prop = subkey.replace(reArray, '');
        reArray.lastIndex = 0;
        arrayMatch = reArray.exec(subkey);
        if (arrayMatch) {
            while (true) {
                if (!_.isArray(innerobj[prop])) {
                    innerobj[prop] = [];
                }
                innerobj = innerobj[prop];
                prop = parseInt(arrayMatch[1], 10);
                arrayMatch = reArray.exec(subkey);
                if (!arrayMatch) break;
            }
            innerobj[prop] = value;
        }
        else {
            innerobj[prop] = value;
        }
    };



    var getSchemaKey = function (schema, key) {
        var schemaKey = key
          .replace(/\./g, '.properties.')
          .replace(/\[[0-9]*\]/g, '.items');
        var schemaDef = jsonform.util.getObjKey(schema, schemaKey, true);
        if (schemaDef && schemaDef.$ref) {
            throw new Error('JSONForm does not yet support schemas that use the ' +
              '$ref keyword. See: https://github.com/joshfire/jsonform/issues/54');
        }
        return schemaDef;
    };


    var truncateToArrayDepth = function (key, arrayDepth) {
        var depth = 0;
        var pos = 0;
        if (!key) return null;

        if (arrayDepth > 0) {
            while (depth < arrayDepth) {
                pos = key.indexOf('[]', pos);
                if (pos === -1) {
                    // Key path is not "deep" enough, simply return the full key
                    return key;
                }
                pos = pos + 2;
                depth += 1;
            }
        }

        // Move one step further to the right without including the final []
        pos = key.indexOf('[]', pos);
        if (pos === -1) return key;
        else return key.substring(0, pos);
    };


    var applyArrayPath = function (key, arrayPath) {
        var depth = 0;
        if (!key) return null;
        if (!arrayPath || (arrayPath.length === 0)) return key;
        var newKey = key.replace(reArray, function (str, p1) {
            // Note this function gets called as many times as there are [x] in the ID,
            // from left to right in the string. The goal is to replace the [x] with
            // the appropriate index in the new array path, if defined.
            var newIndex = str;
            if (isSet(arrayPath[depth])) {
                newIndex = '[' + arrayPath[depth] + ']';
            }
            depth += 1;
            return newIndex;
        });
        return newKey;
    };



    var getInitialValue = function (formObject, key, arrayPath, tpldata, usePreviousValues) {
        var value = null;


        tpldata = tpldata || {};
        tpldata.idx = tpldata.idx ||
          (arrayPath ? arrayPath[arrayPath.length - 1] : 1);
        tpldata.value = isSet(tpldata.value) ? tpldata.value : '';
        tpldata.getValue = tpldata.getValue || function (key) {
            return getInitialValue(formObject, key, arrayPath, tpldata, usePreviousValues);
        };


        var getFormElement = function (elements, key) {
            var formElement = null;
            if (!elements || !elements.length) return null;
            _.each(elements, function (elt) {
                if (formElement) return;
                if (elt === key) {
                    formElement = { key: elt };
                    return;
                }
                if (_.isString(elt)) return;
                if (elt.key === key) {
                    formElement = elt;
                }
                else if (elt.items) {
                    formElement = getFormElement(elt.items, key);
                }
            });
            return formElement;
        };
        var formElement = getFormElement(formObject.form || [], key);
        var schemaElement = getSchemaKey(formObject.schema.properties, key);

        if (usePreviousValues && formObject.value) {

            value = jsonform.util.getObjKey(formObject.value, applyArrayPath(key, arrayPath));
        }
        if (!isSet(value)) {
            if (formElement && (typeof formElement['value'] !== 'undefined')) {

                value = formElement['value'];
            }
            else if (schemaElement) {

                if (isSet(schemaElement['default'])) {
                    value = schemaElement['default'];
                }
            }
            if (value && value.indexOf('{{values.') !== -1) {

                value = value.replace(
                  /\{\{values\.([^\}]+)\}\}/g,
                  '{{getValue("$1")}}');
            }
            if (value) {
                value = _.template(value, tpldata, valueTemplateSettings);
            }
        }

        // Apply titleMap if needed
        if (isSet(value) && formElement &&
            formElement.titleMap &&
            formElement.titleMap[value]) {
            value = _.template(formElement.titleMap[value],
              tpldata, valueTemplateSettings);
        }

        // Check maximum length of a string
        if (value && _.isString(value) &&
          schemaElement && schemaElement.maxLength) {
            if (value.length > schemaElement.maxLength) {
                // Truncate value to maximum length, adding continuation dots
                value = value.substr(0, schemaElement.maxLength - 1) + '…';
            }
        }

        if (!isSet(value)) {
            return null;
        }
        else {
            return value;
        }
    };



    var formNode = function () {

        this.id = null;


        this.key = null;


        this.el = null;


        this.formElement = null;


        this.schemaElement = null;


        this.view = null;


        this.children = [];


        this.ownerTree = null;


        this.parentNode = null;


        this.childTemplate = null;



        this.legendChild = null;



        this.arrayPath = [];


        this.childPos = 0;

        this.dataAttr = null;
    };



    formNode.prototype.clone = function (parentNode) {
        var node = new formNode();
        node.arrayPath = _.clone(this.arrayPath);
        node.ownerTree = this.ownerTree;
        node.parentNode = parentNode || this.parentNode;
        node.formElement = this.formElement;
        node.schemaElement = this.schemaElement;
        node.view = this.view;
        node.dataAttr = this.dataAttr;
        node.children = _.map(this.children, function (child) {
            return child.clone(node);
        });
        if (this.childTemplate) {
            node.childTemplate = this.childTemplate.clone(node);
        }
        return node;
    };



    formNode.prototype.hasNonDefaultValue = function () {

        // hidden elements don't count because they could make the wrong selectfieldset element active
        if (this.formElement && this.formElement.type == "hidden") {
            return false;
        }

        if (this.value && !this.defaultValue) {
            return true;
        }
        var child = _.find(this.children, function (child) {
            return child.hasNonDefaultValue();
        });
        return !!child;
    };



    formNode.prototype.appendChild = function (node) {
        node.parentNode = this;
        node.childPos = this.children.length;
        this.children.push(node);
        return node;
    };


    formNode.prototype.removeChild = function () {
        var child = this.children[this.children.length - 1];
        if (!child) return;

        // Remove the child from the DOM
        $(child.el).remove();

        // Remove the child from the array
        return this.children.pop();
    };



    formNode.prototype.moveValuesTo = function (node) {
        var values = this.getFormValues(node.arrayPath);
        node.resetValues();
        node.computeInitialValues(values, true);
    };



    formNode.prototype.switchValuesWith = function (node) {
        var values = this.getFormValues(node.arrayPath);
        var nodeValues = node.getFormValues(this.arrayPath);
        node.resetValues();
        node.computeInitialValues(values, true);
        this.resetValues();
        this.computeInitialValues(nodeValues, true);
    };



    formNode.prototype.resetValues = function () {
        var params = null;
        var idx = 0;
        this.value = null;

        if (this.parentNode) {
            this.arrayPath = _.clone(this.parentNode.arrayPath);
            if (this.parentNode.view && this.parentNode.view.array) {
                this.arrayPath.push(this.childPos);
            }
        }
        else {
            this.arrayPath = [];
        }

        if (this.view && this.view.inputfield) {
            params = $(':input', this.el).serializeArray();
            _.each(params, function (param) {
                var element = $('[name="' + escapeSelector(param.name) + '"]', $(this.el));
                element.val('');
                removeValidation(element);
            }, this);
        }
        else if (this.view && this.view.array) {
            while (this.children.length > 0) {
                this.removeChild();
            }
        }


        _.each(this.children, function (child) {
            child.resetValues();
        });
    };

    function removeValidation(element) {

        var eltype = element.prop('type');
        var elParent = element.closest('div.form-group');
        elParent.removeClass('has-error');
        elParent.find('span.form-error').remove();
        element.css('border-color', '');
        element.removeClass('error');

        if (eltype == "select-one") {
            element.prop('selectedIndex', 0);
            element.change();
        }
    };

    formNode.prototype.setChildTemplate = function (node) {
        this.childTemplate = node;
        node.parentNode = this;
    };



    formNode.prototype.computeInitialValues = function (values, ignoreDefaultValues) {
        var self = this;
        var node = null;
        var nbChildren = 1;
        var i = 0;
        var formData = this.ownerTree.formDesc.tpldata || {};

        // Propagate the array path from the parent node
        // (adding the position of the child for nodes that are direct
        // children of array-like nodes)
        if (this.parentNode) {
            this.arrayPath = _.clone(this.parentNode.arrayPath);
            if (this.parentNode.view && this.parentNode.view.array) {
                this.arrayPath.push(this.childPos);
            }
        }
        else {
            this.arrayPath = [];
        }

        // Prepare special data param "idx" for templated values
        // (is is the index of the child in its wrapping array, starting
        // at 1 since that's more human-friendly than a zero-based index)
        formData.idx = (this.arrayPath.length > 0) ?
          this.arrayPath[this.arrayPath.length - 1] + 1 :
          this.childPos + 1;

        // Prepare special data param "value" for templated values
        formData.value = '';

        // Prepare special function to compute the value of another field
        formData.getValue = function (key) {
            return getInitialValue(self.ownerTree.formDesc,
              key, self.arrayPath,
              formData, !!values);
        };

        if (this.formElement) {
            // Compute the ID of the field (if needed)
            if (this.formElement.id) {
                this.id = applyArrayPath(this.formElement.id, this.arrayPath);
            }
            else if (this.view && this.view.array) {
                this.id = escapeSelector(this.ownerTree.formDesc.prefix) +
                  '-elt-counter-' + _.uniqueId();
            }
            else if (this.parentNode && this.parentNode.view &&
              this.parentNode.view.array) {
                // Array items need an array to associate the right DOM element
                // to the form node when the parent is rendered.
                this.id = escapeSelector(this.ownerTree.formDesc.prefix) +
                  '-elt-counter-' + _.uniqueId();
            }
            else if ((this.formElement.type === 'button') ||
              (this.formElement.type === 'selectfieldset') ||
              (this.formElement.type === 'question') ||
              (this.formElement.type === 'buttonquestion')) {
                // Buttons do need an id for "onClick" purpose
                this.id = escapeSelector(this.ownerTree.formDesc.prefix) +
                  '-elt-counter-' + _.uniqueId();
            }

            // Compute the actual key (the form element's key is index-free,
            // i.e. it looks like foo[].bar.baz[].truc, so we need to apply
            // the array path of the node to get foo[4].bar.baz[2].truc)
            if (this.formElement.key) {
                this.key = applyArrayPath(this.formElement.key, this.arrayPath);
                this.keydash = this.key.replace(/\./g, '---');
            }

            // Same idea for the field's name
            this.name = applyArrayPath(this.formElement.name, this.arrayPath);

            // Consider that label values are template values and apply the
            // form's data appropriately (note we also apply the array path
            // although that probably doesn't make much sense for labels...)
            _.each([
              'title',
              'legend',
              'description',
              'append',
              'prepend',
              'inlinetitle',
              'helpvalue',
              'value',
              'disabled',
              'placeholder',
              'multiple',
              'readOnly'
            ], function (prop) {
                if (_.isString(this.formElement[prop])) {
                    if (this.formElement[prop].indexOf('{{values.') !== -1) {
                        // This label wants to use the value of another input field.
                        // Convert that construct into {{jsonform.getValue(key)}} for
                        // Underscore to call the appropriate function of formData
                        // when template gets called (note calling a function is not
                        // exactly Mustache-friendly but is supported by Underscore).
                        this[prop] = this.formElement[prop].replace(
                          /\{\{values\.([^\}]+)\}\}/g,
                          '{{getValue("$1")}}');
                    }
                    else {
                        // Note applying the array path probably doesn't make any sense,
                        // but some geek might want to have a label "foo[].bar[].baz",
                        // with the [] replaced by the appropriate array path.
                        this[prop] = applyArrayPath(this.formElement[prop], this.arrayPath);
                    }
                    if (this[prop]) {
                        this[prop] = _.template(this[prop], formData, valueTemplateSettings);
                    }
                }
                else {
                    this[prop] = this.formElement[prop];
                }
            }, this);

            // Apply templating to options created with "titleMap" as well
            if (this.formElement.options) {
                this.options = _.map(this.formElement.options, function (option) {
                    var title = null;
                    if (_.isObject(option) && option.title) {
                        // See a few lines above for more details about templating
                        // preparation here.
                        if (option.title.indexOf('{{values.') !== -1) {
                            title = option.title.replace(
                              /\{\{values\.([^\}]+)\}\}/g,
                              '{{getValue("$1")}}');
                        }
                        else {
                            title = applyArrayPath(option.title, self.arrayPath);
                        }
                        return _.extend({}, option, {
                            value: (isSet(option.value) ? option.value : ''),
                            title: _.template(title, formData, valueTemplateSettings)
                        });
                    }
                    else {
                        return option;
                    }
                });
            }
        }

        if (this.view && this.view.inputfield && this.schemaElement) {
            if (values) {
                if (isSet(jsonform.util.getObjKey(values, this.key))) {
                    this.value = jsonform.util.getObjKey(values, this.key);
                }
            }
            else if (!ignoreDefaultValues) {
                if (!isSet(this.value) && isSet(this.schemaElement['default'])) {
                    this.value = this.schemaElement['default'];
                    if (_.isString(this.value)) {
                        if (this.value.indexOf('{{values.') !== -1) {
                            this.value = this.value.replace(
                              /\{\{values\.([^\}]+)\}\}/g,
                              '{{getValue("$1")}}');
                        }
                        else {
                            this.value = applyArrayPath(this.value, this.arrayPath);
                        }
                        if (this.value) {
                            this.value = _.template(this.value, formData, valueTemplateSettings);
                        }
                    }
                    this.defaultValue = true;
                }
            }
        }
        else if (this.view && this.view.array) {
            nbChildren = 0;
            if (values) {
                nbChildren = this.getPreviousNumberOfItems(values, this.arrayPath);
            }
            else if (nbChildren === 0) {
                nbChildren = 1;
            }
            for (i = 0; i < nbChildren; i++) {
                this.appendChild(this.childTemplate.clone());
            }
        }


        _.each(this.children, function (child) {
            child.computeInitialValues(values, ignoreDefaultValues);
        });

        if (this.formElement && this.formElement.valueInLegend) {
            node = this;
            while (node) {
                if (node.parentNode &&
                  node.parentNode.view &&
                  node.parentNode.view.array) {
                    node.legendChild = this;
                    if (node.formElement && node.formElement.legend) {
                        node.legend = applyArrayPath(node.formElement.legend, node.arrayPath);
                        formData.idx = (node.arrayPath.length > 0) ?
                          node.arrayPath[node.arrayPath.length - 1] + 1 :
                          node.childPos + 1;
                        formData.value = isSet(this.value) ? this.value : '';
                        node.legend = _.template(node.legend, formData, valueTemplateSettings);
                        break;
                    }
                }
                node = node.parentNode;
            }
        }
    };



    formNode.prototype.getPreviousNumberOfItems = function (values, arrayPath) {
        var key = null;
        var arrayValue = null;
        var childNumbers = null;
        var idx = 0;

        if (!values) {

            return 0;
        }

        if (this.view.inputfield && this.schemaElement) {
            key = truncateToArrayDepth(this.formElement.key, arrayPath.length);
            key = applyArrayPath(key, arrayPath);
            arrayValue = jsonform.util.getObjKey(values, key);
            if (!arrayValue) {
                return 0;
            }
            childNumbers = _.map(this.children, function (child) {
                return child.getPreviousNumberOfItems(values, arrayPath);
            });
            return _.max([_.max(childNumbers) || 0, arrayValue.length]);
        }
        else if (this.view.array) {
            return this.childTemplate.getPreviousNumberOfItems(values, arrayPath);
        }
        else {
            childNumbers = _.map(this.children, function (child) {
                return child.getPreviousNumberOfItems(values, arrayPath);
            });
            return _.max(childNumbers) || 0;
        }
    };



    formNode.prototype.getFormValues = function (updateArrayPath) {

        var values = {};

        if (!this.el) {
            throw new Error('formNode.getFormValues can only be called on nodes that are associated with a DOM element in the tree');
        }


        var formArray = $(':input', this.el).serializeArray();


        formArray = formArray.concat(
          $(':input[type=checkbox]:not(:disabled):not(:checked)', this.el).map(function () {
              return { "name": this.name, "value": this.checked }
          }).get()
        );

        if (updateArrayPath) {
            _.each(formArray, function (param) {
                param.name = applyArrayPath(param.name, updateArrayPath);
            });
        }


        var formSchema = this.ownerTree.formDesc.schema;

        for (var i = 0; i < formArray.length; i++) {

            var name = formArray[i].name;
            var eltSchema = getSchemaKey(formSchema.properties, name);
            var arrayMatch = null;
            var cval = null;


            if (!eltSchema) continue;

            if (eltSchema._jsonform_checkboxes_as_array) {
                arrayMatch = name.match(/\[([0-9]*)\]$/);
                if (arrayMatch) {
                    name = name.replace(/\[([0-9]*)\]$/, '');
                    cval = jsonform.util.getObjKey(values, name) || [];
                    if (formArray[i].value === '1') {

                        cval.push(eltSchema['enum'][parseInt(arrayMatch[1], 10)]);
                    }
                    jsonform.util.setObjKey(values, name, cval);
                    continue;
                }
            }


            if (eltSchema.type === 'boolean') {
                if (formArray[i].value === '0') {
                    formArray[i].value = false;
                } else {
                    formArray[i].value = !!formArray[i].value;
                }
            }
            if ((eltSchema.type === 'number') ||
              (eltSchema.type === 'integer')) {
                if (_.isString(formArray[i].value)) {
                    if (!formArray[i].value.length) {
                        formArray[i].value = null;
                    } else if (!isNaN(Number(formArray[i].value))) {
                        formArray[i].value = Number(formArray[i].value);
                    }
                }
            }
            if ((eltSchema.type === 'string') &&
              (formArray[i].value === '') &&
              !eltSchema._jsonform_allowEmpty) {
                formArray[i].value = null;
            }
            if ((eltSchema.type === 'object') &&
              _.isString(formArray[i].value) &&
              (formArray[i].value.substring(0, 1) === '{')) {
                try {
                    formArray[i].value = JSON.parse(formArray[i].value);
                } catch (e) {
                    formArray[i].value = {};
                }
            }
            //TODO is this due to a serialization bug?
            if ((eltSchema.type === 'object') &&
              (formArray[i].value === 'null' || formArray[i].value === '')) {
                formArray[i].value = null;
            }

            if (formArray[i].name && (formArray[i].value !== null)) {
                jsonform.util.setObjKey(values, formArray[i].name, formArray[i].value);
            }
        }
        // console.log("Form value",values);
        return values;
    };




    formNode.prototype.render = function (el) {
        var html = this.generate();
        this.setContent(html, el);
        this.enhance();
    };



    formNode.prototype.setContent = function (html, parentEl) {
        var node = $(html);
        var parentNode = parentEl ||
          (this.parentNode ? this.parentNode.el : this.ownerTree.domRoot);
        var nextSibling = null;

        if (this.el) {
            // Replace the contents of the DOM element if the node is already in the tree
            $(this.el).replaceWith(node);
        }
        else {
            // Insert the node in the DOM if it's not already there
            nextSibling = $(parentNode).children().get(this.childPos);
            if (nextSibling) {
                $(nextSibling).before(node);
            }
            else {
                $(parentNode).append(node);
            }
        }

        // Save the link between the form node and the generated HTML
        this.el = node;

        // Update the node's subtree, extracting DOM elements that match the nodes
        // from the generated HTML
        this.updateElement(this.el);
    };


    formNode.prototype.updateElement = function (domNode) {
        if (this.id) {
            this.el = $('#' + escapeSelector(this.id), domNode).get(0);
            if (this.view && this.view.getElement) {
                this.el = this.view.getElement(this.el);
            }
            if ((this.fieldtemplate !== false) &&
              this.view && this.view.fieldtemplate) {
                // The field template wraps the element two or three level deep
                // in the DOM tree, depending on whether there is anything prepended
                // or appended to the input field
                this.el = $(this.el).parent().parent();
                if (this.prepend || this.prepend) {
                    this.el = this.el.parent();
                }
                this.el = this.el.get(0);
            }
            if (this.parentNode && this.parentNode.view &&
              this.parentNode.view.childTemplate) {
                // TODO: the child template may introduce more than one level,
                // so the number of levels introduced should rather be exposed
                // somehow in jsonform.fieldtemplate.
                this.el = $(this.el).parent().get(0);
            }
        }

        _.each(this.children, function (child) {
            child.updateElement(this.el || domNode);
        });
    };


    formNode.prototype.generate = function () {
        var data = {
            id: this.id,
            keydash: this.keydash,
            elt: this.formElement,
            schema: this.schemaElement,
            node: this,
            value: isSet(this.value) ? this.value : '',
            escape: escapeHTML,
            attribute: dataAttributes,
        };
        var template = null;
        var html = '';

        // Complete the data context if needed
        if (this.ownerTree.formDesc.onBeforeRender) {
            this.ownerTree.formDesc.onBeforeRender(data, this);
        }
        if (this.view.onBeforeRender) {
            this.view.onBeforeRender(data, this);
        }

        // Use the template that 'onBeforeRender' may have set,
        // falling back to that of the form element otherwise
        if (this.template) {
            template = this.template;
        }
        else if (this.formElement && this.formElement.template) {
            template = this.formElement.template;
        }
        else {
            template = this.view.template;
        }

        // Wrap the view template in the generic field template
        // (note the strict equality to 'false', needed as we fallback
        // to the view's setting otherwise)
        if ((this.fieldtemplate !== false) &&
          (this.fieldtemplate || this.view.fieldtemplate)) {
            template = jsonform.fieldTemplate(template);
        }

        // Wrap the content in the child template of its parent if necessary.
        if (this.parentNode && this.parentNode.view &&
          this.parentNode.view.childTemplate) {
            template = this.parentNode.view.childTemplate(template);
        }

        // Prepare the HTML of the children
        var childrenhtml = '';
        _.each(this.children, function (child) {
            childrenhtml += child.generate();
        });
        data.children = childrenhtml;

        data.fieldHtmlClass = '';
        if (this.ownerTree &&
            this.ownerTree.formDesc &&
            this.ownerTree.formDesc.params &&
            this.ownerTree.formDesc.params.fieldHtmlClass) {
            data.fieldHtmlClass = this.ownerTree.formDesc.params.fieldHtmlClass;
        }
        if (this.formElement &&
            (typeof this.formElement.fieldHtmlClass !== 'undefined')) {
            data.fieldHtmlClass = this.formElement.fieldHtmlClass;
        }

        // Apply the HTML template
        html = _.template(template, data, fieldTemplateSettings);
        return html;
    };


    formNode.prototype.enhance = function () {
        var node = this;
        var handlers = null;
        var handler = null;
        var formData = _.clone(this.ownerTree.formDesc.tpldata) || {};

        if (this.formElement) {
            // Check the view associated with the node as it may define an "onInsert"
            // event handler to be run right away
            if (this.view.onInsert) {
                this.view.onInsert({ target: $(this.el) }, this);
            }

            handlers = this.handlers || this.formElement.handlers;

            // Trigger the "insert" event handler
            handler = this.onInsert || this.formElement.onInsert;
            if (handler) {
                handler({ target: $(this.el) }, this);
            }
            if (handlers) {
                _.each(handlers, function (handler, onevent) {
                    if (onevent === 'insert') {
                        handler({ target: $(this.el) }, this);
                    }
                }, this);
            }

            // No way to register event handlers if the DOM element is unknown
            // TODO: find some way to register event handlers even when this.el is not set.
            if (this.el) {

                // Register specific event handlers
                // TODO: Add support for other event handlers
                if (this.onChange)
                    $(this.el).bind('change', function (evt) { node.onChange(evt, node); });
                if (this.view.onChange)
                    $(this.el).bind('change', function (evt) { node.view.onChange(evt, node); });
                if (this.formElement.onChange)
                    $(this.el).bind('change', function (evt) { node.formElement.onChange(evt, node); });

                if (this.onClick)
                    $(this.el).bind('click', function (evt) { node.onClick(evt, node); });
                if (this.view.onClick)
                    $(this.el).bind('click', function (evt) { node.view.onClick(evt, node); });
                if (this.formElement.onClick)
                    $(this.el).bind('click', function (evt) { node.formElement.onClick(evt, node); });

                if (this.onKeyUp)
                    $(this.el).bind('keyup', function (evt) { node.onKeyUp(evt, node); });
                if (this.view.onKeyUp)
                    $(this.el).bind('keyup', function (evt) { node.view.onKeyUp(evt, node); });
                if (this.formElement.onKeyUp)
                    $(this.el).bind('keyup', function (evt) { node.formElement.onKeyUp(evt, node); });

                if (handlers) {
                    _.each(handlers, function (handler, onevent) {
                        if (onevent !== 'insert') {
                            $(this.el).bind(onevent, function (evt) { handler(evt, node); });
                        }
                    }, this);
                }
            }

            // Auto-update legend based on the input field that's associated with it
            if (this.legendChild && this.legendChild.formElement) {
                $(this.legendChild.el).bind('keyup', function (evt) {
                    if (node.formElement && node.formElement.legend && node.parentNode) {
                        node.legend = applyArrayPath(node.formElement.legend, node.arrayPath);
                        formData.idx = (node.arrayPath.length > 0) ?
                          node.arrayPath[node.arrayPath.length - 1] + 1 :
                          node.childPos + 1;
                        formData.value = $(evt.target).val();
                        node.legend = _.template(node.legend, formData, valueTemplateSettings);
                        $(node.parentNode.el).trigger('legendUpdated');
                    }
                });
            }
        }

        // Recurse down the tree to enhance children
        _.each(this.children, function (child) {
            child.enhance();
        });
    };

    formNode.prototype.insertArrayItem = function (idx, domElement) {
        var i = 0;

        // Insert element at the end of the array if index is not given
        if (idx === undefined) {
            idx = this.children.length;
        }

        // Create the additional array item at the end of the list,
        // using the item template created when tree was initialized
        // (the call to resetValues ensures that 'arrayPath' is correctly set)
        var child = this.childTemplate.clone();
        this.appendChild(child);
        child.resetValues();

        // To create a blank array item at the requested position,
        // shift values down starting at the requested position
        // one to insert (note we start with the end of the array on purpose)
        for (i = this.children.length - 2; i >= idx; i--) {
            this.children[i].moveValuesTo(this.children[i + 1]);
        }

        // Initialize the blank node we've created with default values
        this.children[idx].resetValues();
        this.children[idx].computeInitialValues();

        // Re-render all children that have changed
        for (i = idx; i < this.children.length; i++) {
            this.children[i].render(domElement);
        }
    };


    formNode.prototype.deleteArrayItem = function (idx) {
        var i = 0;
        var child = null;

        // Delete last item if no index is given
        if (idx === undefined) {
            idx = this.children.length - 1;
        }

        // Move values up in the array
        for (i = idx; i < this.children.length - 1; i++) {
            this.children[i + 1].moveValuesTo(this.children[i]);
            this.children[i].render();
        }

        // Remove the last array item from the DOM tree and from the form tree
        this.removeChild();
    };


    formNode.prototype.getArrayBoundaries = function () {
        var boundaries = {
            minItems: -1,
            maxItems: -1
        };
        if (!this.view || !this.view.array) return boundaries;

        var getNodeBoundaries = function (node, initialNode) {
            var schemaKey = null;
            var arrayKey = null;
            var boundaries = {
                minItems: -1,
                maxItems: -1
            };
            initialNode = initialNode || node;

            if (node.view && node.view.array && (node !== initialNode)) {
                // New array level not linked to an array in the schema,
                // so no size constraints
                return boundaries;
            }

            if (node.key) {
                // Note the conversion to target the actual array definition in the
                // schema where minItems/maxItems may be defined. If we're still looking
                // at the initial node, the goal is to convert from:
                //  foo[0].bar[3].baz to foo[].bar[].baz
                // If we're not looking at the initial node, the goal is to look at the
                // closest array parent:
                //  foo[0].bar[3].baz to foo[].bar
                arrayKey = node.key.replace(/\[[0-9]+\]/g, '[]');
                if (node !== initialNode) {
                    arrayKey = arrayKey.replace(/\[\][^\[\]]*$/, '');
                }
                schemaKey = getSchemaKey(
                  node.ownerTree.formDesc.schema.properties,
                  arrayKey
                );
                if (!schemaKey) return boundaries;
                return {
                    minItems: schemaKey.minItems || schemaKey.minLength || -1,
                    maxItems: schemaKey.maxItems || schemaKey.maxLength || -1
                };
            }
            else {
                _.each(node.children, function (child) {
                    var subBoundaries = getNodeBoundaries(child, initialNode);
                    if (subBoundaries.minItems !== -1) {
                        if (boundaries.minItems !== -1) {
                            boundaries.minItems = Math.max(
                              boundaries.minItems,
                              subBoundaries.minItems
                            );
                        }
                        else {
                            boundaries.minItems = subBoundaries.minItems;
                        }
                    }
                    if (subBoundaries.maxItems !== -1) {
                        if (boundaries.maxItems !== -1) {
                            boundaries.maxItems = Math.min(
                              boundaries.maxItems,
                              subBoundaries.maxItems
                            );
                        }
                        else {
                            boundaries.maxItems = subBoundaries.maxItems;
                        }
                    }
                });
            }
            return boundaries;
        };
        return getNodeBoundaries(this);
    };



    var formTree = function () {
        this.eventhandlers = [];
        this.root = null;
        this.formDesc = null;
    };


    formTree.prototype.initialize = function (formDesc) {
        formDesc = formDesc || {};

        // Keep a pointer to the initial JSONForm
        // (note clone returns a shallow copy, only first-level is cloned)
        this.formDesc = _.clone(formDesc);

        // Compute form prefix if no prefix is given.
        this.formDesc.prefix = this.formDesc.prefix ||
          'jsonform-' + _.uniqueId();

        // JSON schema shorthand
        if (this.formDesc.schema && !this.formDesc.schema.properties) {
            this.formDesc.schema = {
                properties: this.formDesc.schema
            };
        }

        // Ensure layout is set
        this.formDesc.form = this.formDesc.form || [
          '*',
          {
              type: 'actions',
              items: [
                {
                    type: 'submit',
                    value: 'Submit'
                }
              ]
          }
        ];
        this.formDesc.form = (_.isArray(this.formDesc.form) ?
          this.formDesc.form :
          [this.formDesc.form]);

        this.formDesc.params = this.formDesc.params || {};

        // Create the root of the tree
        this.root = new formNode();
        this.root.ownerTree = this;
        this.root.view = jsonform.elementTypes['root'];

        // Generate the tree from the form description
        this.buildTree();

        // Compute the values associated with each node
        // (for arrays, the computation actually creates the form nodes)
        this.computeInitialValues();
    };

    formTree.prototype.buildTree = function () {
        // Parse and generate the form structure based on the elements encountered:
        // - '*' means "generate all possible fields using default layout"
        // - a key reference to target a specific data element
        // - a more complex object to generate specific form sections
        _.each(this.formDesc.form, function (formElement) {
            if (formElement === '*') {
                _.each(this.formDesc.schema.properties, function (element, key) {
                    this.root.appendChild(this.buildFromLayout({
                        key: key
                    }));
                }, this);
            }
            else {
                if (_.isString(formElement)) {
                    formElement = {
                        key: formElement
                    };
                }
                this.root.appendChild(this.buildFromLayout(formElement));
            }
        }, this);
    };

    formTree.prototype.buildFromLayout = function (formElement, context) {
        var schemaElement = null;
        var node = new formNode();
        var view = null;
        var key = null;


        formElement = _.clone(formElement);
        if (formElement.items) {
            if (_.isArray(formElement.items)) {
                formElement.items = _.map(formElement.items, _.clone);
            }
            else {
                formElement.items = [_.clone(formElement.items)];
            }
        }

        if (formElement.key) {

            schemaElement = getSchemaKey(
              this.formDesc.schema.properties,
              formElement.key);
            if (!schemaElement) {
                // The JSON Form is invalid!
                throw new Error('The JSONForm object references the schema key "' +
                  formElement.key + '" but that key does not exist in the JSON schema');
            }

            // Schema element has just been found, let's trigger the
            // "onElementSchema" event
            // (tidoust: not sure what the use case for this is, keeping the
            // code for backward compatibility)
            if (this.formDesc.onElementSchema) {
                this.formDesc.onElementSchema(formElement, schemaElement);
            }

            formElement.name =
              formElement.name ||
              formElement.key;
            formElement.title =
              formElement.title ||
              schemaElement.title;
            formElement.description =
              formElement.description ||
              schemaElement.description;
            formElement.readOnly =
              formElement.readOnly ||
              schemaElement.readOnly ||
              formElement.readonly ||
              schemaElement.readonly;

            // Compute the ID of the input field
            if (!formElement.id) {
                formElement.id = escapeSelector(this.formDesc.prefix) +
                  '-elt-' + formElement.key;
            }

            // Should empty strings be included in the final value?
            // TODO: it's rather unclean to pass it through the schema.
            if (formElement.allowEmpty) {
                schemaElement._jsonform_allowEmpty = true;
            }

            // If the form element does not define its type, use the type of
            // the schema element.
            if (!formElement.type) {
                if ((schemaElement.type === 'string') &&
                  (schemaElement.format === 'color')) {
                    formElement.type = 'color';
                } else if ((schemaElement.type === 'number' ||
                    schemaElement.type === 'integer' ||
                    schemaElement.type === 'string' ||
                    schemaElement.type === 'any') &&
                  !schemaElement['enum']) {
                    formElement.type = 'text';
                } else if (schemaElement.type === 'boolean') {
                    formElement.type = 'checkbox';
                } else if (schemaElement.type === 'object') {
                    if (schemaElement.properties) {
                        formElement.type = 'fieldset';
                    } else {
                        formElement.type = 'textarea';
                    }
                } else if (!_.isUndefined(schemaElement['enum'])) {
                    formElement.type = 'select';
                } else {
                    formElement.type = schemaElement.type;
                }
            }

            // Unless overridden in the definition of the form element (or unless
            // there's a titleMap defined), use the enumeration list defined in
            // the schema
            if (!formElement.options && schemaElement['enum']) {
                if (formElement.titleMap) {
                    formElement.options = _.map(schemaElement['enum'], function (value) {
                        return {
                            value: value,
                            title: formElement.titleMap[value] || value
                        };
                    });
                }
                else {
                    formElement.options = schemaElement['enum'];
                }
            }

            // Flag a list of checkboxes with multiple choices
            if ((formElement.type === 'checkboxes') && schemaElement.items) {
                var itemsEnum = schemaElement.items['enum'];
                if (itemsEnum) {
                    schemaElement.items._jsonform_checkboxes_as_array = true;
                }
                if (!itemsEnum && schemaElement.items[0]) {
                    itemsEnum = schemaElement.items[0]['enum'];
                    if (itemsEnum) {
                        schemaElement.items[0]._jsonform_checkboxes_as_array = true;
                    }
                }
            }

            // If the form element targets an "object" in the JSON schema,
            // we need to recurse through the list of children to create an
            // input field per child property of the object in the JSON schema
            if (schemaElement.type === 'object') {
                _.each(schemaElement.properties, function (prop, propName) {
                    node.appendChild(this.buildFromLayout({
                        key: formElement.key + '.' + propName
                    }));
                }, this);
            }
        }

        if (!formElement.type) {
            formElement.type = 'none';
        }
        view = jsonform.elementTypes[formElement.type];
        if (!view) {
            throw new Error('The JSONForm contains an element whose type is unknown: "' +
              formElement.type + '"');
        }


        if (schemaElement) {
            // The form element is linked to an element in the schema.
            // Let's make sure the types are compatible.
            // In particular, the element must not be a "container"
            // (or must be an "object" or "array" container)
            if (!view.inputfield && !view.array &&
              (formElement.type !== 'selectfieldset') &&
              (schemaElement.type !== 'object')) {
                throw new Error('The JSONForm contains an element that links to an ' +
                  'element in the JSON schema (key: "' + formElement.key + '") ' +
                  'and that should not based on its type ("' + formElement.type + '")');
            }
        }
        else {
            // The form element is not linked to an element in the schema.
            // This means the form element must be a "container" element,
            // and must not define an input field.
            if (view.inputfield && (formElement.type !== 'selectfieldset')) {
                throw new Error('The JSONForm defines an element of type ' +
                  '"' + formElement.type + '" ' +
                  'but no "key" property to link the input field to the JSON schema');
            }
        }

        // A few characters need to be escaped to use the ID as jQuery selector
        formElement.iddot = escapeSelector(formElement.id || '');


        node.formElement = formElement;
        node.schemaElement = schemaElement;
        node.view = view;
        node.ownerTree = this;


        if (!formElement.handlers) {
            formElement.handlers = {};
        }


        if (node.view.array) {

            if (formElement.items) {
                key = formElement.items[0] || formElement.items;
            }
            else {
                key = formElement.key + '[]';
            }
            if (_.isString(key)) {
                key = { key: key };
            }
            node.setChildTemplate(this.buildFromLayout(key));
        }
        else if (formElement.items) {
            // The form element defines children elements
            _.each(formElement.items, function (item) {
                if (_.isString(item)) {
                    item = { key: item };
                }
                node.appendChild(this.buildFromLayout(item));
            }, this);
        }

        return node;
    };

    formTree.prototype.computeInitialValues = function () {
        this.root.computeInitialValues(this.formDesc.value);
    };

    formTree.prototype.render = function (domRoot) {
        if (!domRoot) return;
        this.domRoot = domRoot;
        this.root.render();

        // If the schema defines required fields, flag the form with the
        // "jsonform-hasrequired" class for styling purpose
        // (typically so that users may display a legend)
        if (this.hasRequiredField()) {
            $(domRoot).addClass('jsonform-hasrequired');
        }
    };


    formTree.prototype.forEachElement = function (callback) {

        var f = function (root) {
            for (var i = 0; i < root.children.length; i++) {
                callback(root.children[i]);
                f(root.children[i]);
            }
        };
        f(this.root);

    };

    formTree.prototype.validate = function (noErrorDisplay) {
        console.log('validate 01');
        var values = jsonform.getFormValue(this.domRoot);
        var errors = false;

        var options = this.formDesc;

        if (options.validate !== false) {
            var validator = false;
            if (typeof options.validate != "object") {
                if (global.JSONFormValidator) {
                    validator = global.JSONFormValidator.createEnvironment("json-schema-draft-03");
                }
            } else {
                validator = options.validate;
            }
            if (validator) {
                var v = validator.validate(values, this.formDesc.schema);
                console.log('if validator');
                if (v.errors.length) {
                    if (!errors) errors = [];
                    errors = errors.concat(v.errors);
                }
            }
        }

        if (errors && !noErrorDisplay) {
            if (options.displayErrors) {
                options.displayErrors(errors, this.domRoot);
            } else {

            }
        }

        return { "errors": errors }

    }



    formTree.prototype.submit = function (evt) {

        var stopEvent = function () {
            if (evt) {
                evt.preventDefault();
                evt.stopPropagation();
            }
            return false;
        };
        var values = jsonform.getFormValue(this.domRoot);
        var options = this.formDesc;

        var brk = false;
        this.forEachElement(function (elt) {
            console.log(brk);
            if (brk) return;
            var element = elt.el;
            if (null != element) {
                var child = $(element).children('div.form-group');
                if (child.hasClass('has-error')) {
                    console.log('has error');
                }
            }
        });


        return false;

    };

    formTree.prototype.hasRequiredField = function () {
        var parseElement = function (element) {
            if (!element) return null;
            if (element.required && (element.type !== 'boolean')) {
                return element;
            }

            var prop = _.find(element.properties, function (property) {
                return parseElement(property);
            });
            if (prop) {
                return prop;
            }

            if (element.items) {
                if (_.isArray(element.items)) {
                    prop = _.find(element.items, function (item) {
                        return parseElement(item);
                    });
                }
                else {
                    prop = parseElement(element.items);
                }
                if (prop) {
                    return prop;
                }
            }
        };

        return parseElement(this.formDesc.schema);
    };

    formTree.prototype.validateResult = function (evt) {
        var brk = false;
        this.forEachElement(function (elt) {
            if (brk) return;
            var element = elt.el;
            if (null != element) {
                var child = $(element).children('div.form-group');
                if (child.hasClass('has-error')) {
                    brk = true;
                }
            }
        });

        if (brk)
            return false;

        return true;

    };

    jsonform.getFormValue = function (formelt) {
        var form = $(formelt).data('jsonform-tree');
        if (!form) return null;
        return form.root.getFormValues();
    };

    jsonform.resetForm = function (formelt) {
        var form = $(formelt).data('jsonform-tree');
        if (!form) return null;
        return form.root.resetValues();
    };

    jsonform.formValid = function (formelt) {
        var form = $(formelt).data('jsonform-tree');
        if (!form) return null;
        return form.validateResult();
    };
    /*
   $.fn.jsonFormErrors = function (errors, options) {
       $(".error", this).removeClass("error");
       $(".warning", this).removeClass("warning");

       $(".jsonform-errortext", this).hide();
       if (!errors) return;

       var errorSelectors = [];
       for (var i = 0; i < errors.length; i++) {            
           var key = errors[i].uri
             .replace(/.*#\//, '')
             .replace(/\//g, '.')
             .replace(/\.([0-9]+)(?=\.|$)/g, '[$1]');
           var errormarkerclass = ".jsonform-error-" +
             escapeSelector(key.replace(/\./g, "---"));
           errorSelectors.push(errormarkerclass);

           var errorType = errors[i].type || "error";
           $(errormarkerclass, this).addClass(errorType);
           $(errormarkerclass + " .jsonform-errortext", this).html(errors[i].message).show();
       }

        
       errorSelectors = errorSelectors.join(',');
       var firstError = $(errorSelectors).get(0);
       if (firstError && firstError.scrollIntoView) {
           firstError.scrollIntoView(true, {
               behavior: 'smooth'
           });
       }
   };
   */

    $.fn.jsonForm = function (options) {
        var formElt = this;

        options = _.defaults({}, options, { submitEvent: 'submit' });

        var form = new formTree();
        form.initialize(options);
        form.render(formElt.get(0));

        // TODO: move that to formTree.render
        if (options.transloadit) {
            formElt.append('<input type="hidden" name="params" value=\'' +
              escapeHTML(JSON.stringify(options.transloadit.params)) +
              '\'>');
        }

        // Keep a direct pointer to the JSON schema for form submission purpose
        formElt.data("jsonform-tree", form);

        if (options.submitEvent) {
            formElt.unbind((options.submitEvent) + '.jsonform');
            formElt.bind((options.submitEvent) + '.jsonform', function (evt) {
                /*form.submit(evt);*/
            });
        }

        // Initialize tabs sections, if any
        initializeTabs(formElt);

        // Initialize expandable sections, if any
        $('.expandable > div, .expandable > fieldset', formElt).hide();
        $('.expandable > legend', formElt).click(function () {
            var parent = $(this).parent();
            parent.toggleClass('expanded');
            $('> div', parent).slideToggle(100);
        });

        return form;
    };

    $.fn.FormValue = function () {
        return jsonform.getFormValue(this);
    };

    $.fn.FormReset = function () {
        return jsonform.resetForm(this);
    };

    $.fn.FormValid = function () {
        return jsonform.formValid(this);
    };

    global.JSONForm = global.JSONForm || { util: {} };
    global.JSONForm.getFormValue = jsonform.getFormValue;
    global.JSONForm.fieldTemplate = jsonform.fieldTemplate;
    global.JSONForm.fieldTypes = jsonform.elementTypes;
    global.JSONForm.getInitialValue = getInitialValue;
    global.JSONForm.util.getObjKey = jsonform.util.getObjKey;
    global.JSONForm.util.setObjKey = jsonform.util.setObjKey;

})((typeof exports !== 'undefined'),
 ((typeof exports !== 'undefined') ? exports : window),
 ((typeof jQuery !== 'undefined') ? jQuery : { fn: {} }),
 ((typeof _ !== 'undefined') ? _ : null),
 JSON);
