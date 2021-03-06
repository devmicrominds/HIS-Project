/*
* jQuery Multi-Column-Select v0.3
*
* Copyright (c) 2014 DanSmith
*
* Licensed under MIT
*
*/
(function ( $ ) {
 
        $.fn.MultiColumnSelect = function( options ) {
        var args = [];
	$optioncount = 0;
        
	var settings = $.extend(
    {
            multiple:           false,              // Single or Multiple Select- Default Single
            useOptionText :     true,               // Use text from option. Default true, use false if you plan to use images
            hideselect :        true,               // Hide Original Select Control
            openmenuClass :     'mcs-open',         // Toggle Open Button Class
            openmenuText :      'Choose An Option', // Text for button
            openclass :         'open',             // Class added to Toggle button on open
            containerClass :    'mcs-container',    // Class of parent container
            itemClass :         'mcs-item',         // Class of menu items
            idprefix : null,                        // Assign as ID to items eg 'item-' = #item-1, #item-2, #item-3...
            duration : 200,                         //Toggle Height duration
            onOpen : null,
            onClose : null,
            onItemSelect : null
        }, options );
      
       this.find('select').val(0);     
            
       if (settings.hideselect == true)
       {
           this.find('select').hide();
       }else this.find('select').show();
       
       init_msc(settings.openmenuClass, settings.openmenuText, settings.containerClass, settings.multiple, this); //create the wrapper
       
       this.find('select option').each(function(e,v) //get elements in dropdown
       {
           generateitems (this, settings.useOptionText,settings.idprefix,settings.itemClass,settings.containerClass );
       });
        
        this.on('click','.'+settings.itemClass,function(e)    //on menu item click
        {
           $select = $(this).parent().prev().prev();
           if ($select.val() !== null) args = $select.val();
           itemclick(this,settings.itemClass,args);
           if ( $.isFunction( settings.onItemSelect ) ) settings.onItemSelect.call( this ); // Select item :: callback
           e.preventDefault();                        
        });
        
        this.find('.'+settings.openmenuClass).on('click',function(e)
        {
            $menucontainer = $(this).next();
                if ($(this).hasClass(settings.openclass)){         
                    $(this).removeClass(settings.openclass);  
                    $menucontainer.slideToggle( "slow", function() {
                      
                        if ( $.isFunction( settings.onClose ) ) {
                            settings.onClose.call( this );
                        }    
                    });
                }else{                
                    $(this).addClass(settings.openclass);
                   
                    $menucontainer.slideToggle( "slow", function() {
                          
                          if ( $.isFunction( settings.onOpen ) ) {
                            settings.onOpen.call( this );
                          }                         
                   });
                };
                e.preventDefault();                    
        });
    return this;
    };
        
    
    $.fn.MultiColumnSelectDestroy = function() {
    destroymsc(this);
    };
    
    $.fn.MultiColumnSelectAdditem = function( itemvalue, itemtext, idprefix) {
        $mcs = this.find('select'); 
        $count = this.find('select options').size()
        $mcs.append($('<option/>', { value: itemvalue, text : itemtext }));
        $toggle = $mcs.next();
        if($toggle.hasClass('mcs')){
       
            $container = $toggle.next();
            $menuitem = $container.children();
            $menuitemClass = $menuitem.attr('class');
            var idtemplate = ""; if (typeof(idprefix) !== 'undefined') idtemplate = "' id='" + idprefix + $count;
            $newitem = "<a class='"+ $menuitemClass + " additem' data='"+ itemvalue +idtemplate + "'>"+ itemtext + "</a>"
            $container.append($newitem);
        }
    }
    
    //private functions 
    itemclick = function (selector,itemClass, args) {
            $itemdata = $(selector).attr('data');
            $menucontainer = $(selector).parent();
            if ($menucontainer.hasClass('Multi')){
                   if ($(selector).hasClass('active')){		
			//already selected, unselect it
			$(selector).removeClass('active');
                        var removeItem = $itemdata; //ID to be removed
                        args.splice( $.inArray(removeItem,args) , 1 ); //Look up at the ID and remove it	                       
		}else{                    
			$(selector).addClass('active');
			args.push($itemdata); 
                };
                $menucontainer.siblings('select').val(args);
            }
            
            if (!$menucontainer.hasClass('Multi')){
                  $menucontainer.siblings('select').val($itemdata); //bind form value
                    $(selector).siblings('a.'+ itemClass).removeClass('active'); //remove all active states
                    $(selector).addClass('active'); //add new active state to clicked item
            }
          
        }    
    init_msc = function(openmenu, opentext, container, multi, append  ){
        toggle = document.createElement('a');mcscontainer = document.createElement('div');
        $(toggle).addClass(openmenu).addClass('mcs').html(opentext).appendTo(append);
        if (multi == true) $(mcscontainer).addClass('Multi');
        $(mcscontainer).addClass(container).appendTo(append);
    };
    
    generateitems = function(selector, useOptionText,idprefix,itemClass,containerClass ){
        var itemtemplate; var idtemplate = ""; var imagetemplate = "";
           $menucontainer = $(selector).parent();
           $optioncount += 1; var settext = '';
           if (useOptionText == true) settext = $(selector).text();
           if (idprefix !== null) idtemplate = "' id='" + idprefix + $optioncount;
           var imagedata = $(selector).data('image');
            
           if (imagedata !== null && imagedata !== undefined) {
               imagetemplate = '<img src="data:image/gif;base64,' + imagedata + '" alt="" style="width:110px;height:90px;"/>'; 
           }
    

           itemtemplate = "<span class='" + itemClass + "'>" + "<a style='background:none;border:1px solid rgba(21, 16, 16, 1);' class='" + "thumbnail" + "' data-id='" + $(selector).attr('value') + idtemplate + "' data-name='"+ $(selector).data('name') +"'>" + imagetemplate + "<em>" + settext + "</em>" + "</a></span>";
           $menucontainer.siblings('.'+containerClass).append(itemtemplate);           
    };
    
    destroymsc = function (selector) {    
        $mcs = selector.find('select');         
        $mcs.show();            
            if ($mcs.next().hasClass('mcs'))
            {
                 $mcs.next().remove();   
                 $mcs.next().remove();              
            }        
    }        
}( jQuery ));
