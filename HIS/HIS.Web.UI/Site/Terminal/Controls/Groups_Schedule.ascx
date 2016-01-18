<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Groups_Schedule.ascx.cs" Inherits="HIS.Web.UI.Site.Terminal.Controls.Groups_Schedule" %>

<div class="row">
    <div class="col-md-4">
        <section class="widget">
            <h4>Campaigns 
            <span class="pull-right">
                <button id="btnReturn" type="button" class="btn btn-info">Return</button>
            </span>
            </h4>
            <div id="external-events">
            </div>
        </section>
    </div>
    <div class="col-md-8">
        <section class="widget">
            <h4>Schedules
                <span class="pull-right">
                    <button id="btn-save" type="button" class="btn btn-primary">Save Schedule</button>
                </span>
            </h4>

            <div id="calendar"></div>
        </section>
    </div>

</div>
<script type="text/javascript">

    $(function () {
        var clcontainer = $('#external-events');

        __JsonModel(<%= this.JsonData %>);
 

        var snippet = _.template($('#campaign_list_template').html(),
        {
            Campaigns: __jsonModel.Campaigns,
        });

        clcontainer.append(snippet);

        clcontainer.find('div.external-event').each(function () {
            var self = this;

            var eventObject = {
                title: $.trim($(self).text()),
                cid: $(self).data('cid')
            };

            $(self).data('eventObject', eventObject);

            $(self).draggable({
                zIndex: 999,
                revert: true,
                revertDuration: 0
            });


        });


        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        var calendar = $('#calendar').fullCalendar(
        {
            header: {
                left: '',
                center: '',
                right: ''
            },
            slotEventOverlap: false,
            defaultView: 'agendaWeek',
            allDaySlot: false,
            selectable: true,
            selectHelper: true,
            columnFormat: {
                month: 'I',
                week: 'ddd',
                day: 'I'
            },
            select: function (start, end, allDay)
            {
                var $modal = $("#edit-modal"),
                    $btn = $('#create-event');
                $btn.off('click');
                $btn.click(function () {
                    var title = $("#event-name").val();
                    if (title)
                    {
                        calendar.fullCalendar('renderEvent',
                            {
                                title: title,
                                start: start,
                                end: end,
                                allDay: allDay
                            },
                            true
                        );
                    }
                    calendar.fullCalendar('unselect');
                });
                $modal.modal('show');
                calendar.fullCalendar('unselect');
            },
            editable: true,
            droppable: true,
            drop: function (date, allDay) {
                var originalEventObject = $(this).data('eventObject');
                var copiedEventObject = $.extend({}, originalEventObject);
                var end = moment(date).add(2, 'hours');

                copiedEventObject.start = date;
                copiedEventObject.end = end;

                if (!isOverlapping(copiedEventObject)) {
                    $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);
                }

            },
            eventDrop: function (event, delta, revertFunc) {
                if (isOverlapping(event))
                    revertFunc();
            },
            eventResize: function (event, delta, revertFunc) {
                if (isOverlapping(event))
                    revertFunc();
            },
            eventClick: function (event) {
                
            },
            events: __jsonModel.Events,
            eventColor: '#378006'


        });



        function isOverlapping(event) {
            var array = calendar.fullCalendar('clientEvents');
            for (i in array) {
                if (array[i]._id != event._id) {
                    if (!(array[i].start.format() >= event.end.format() || array[i].end.format() <= event.start.format())) {
                        return true;
                    }
                }
            }
            return false;
        }

        $('#btnReturn').on('click', function () {
            var json = {
                Action: 'ReturnGroupList',
            };
            _Post(document.URL, json);
        });

        $('#btn-save').on('click', function () {
            var es = calendar.fullCalendar('clientEvents');
            console.log("Azrul Debug");
            console.log(es);
            var en = [];
            _.each(es, function (value, index) {
                en.push({

                    cid: value.cid,
                    start: value.start,
                    end: value.end
                });
            });
            console.log(en);

            amplify.request(Backbone.REQUEST.GROUPS_SAVE_SCHEDULE, JSON.stringify({ group: __jsonModel.Group, events: en }),
            function (data, status) {
                var json = {
                    Action: 'ReturnGroupList',
                };
                _Post(document.URL, json);

            });

        });

    });

</script>

<script id="campaign_list_template" type="text/html">
    <@ _.each(Campaigns,function(o,e){ @> 
    <div data-cid="<@=o.cid @>" class="external-event ui-draggable" style="position: relative;">
        <@=o.name@>
    </div>
    <@});@>
</script>
