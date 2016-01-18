<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditableText.ascx.cs" Inherits="HIS.Web.UI.EditableText" %>
<a href="#"  id="<%= DataID %>" data-type="text"><%= DataValue %></a>

<script type="text/javascript">
   
    $(function () {

        var editabletext =
        {

            initialize: function ()
            {
                $('#' + '<%= DataID %>').editable({
                    type: 'text',
                    mode: 'inline',
                    tpl: '<input type="text" maxlength="<%= this.MaxLength %>"/>',
                    inputclass: '<%= !String.IsNullOrEmpty(this.InputClass) ? this.InputClass : "input-md" %>',
                    validate: function (value) {
                        <%= this.Validate %>
                    }
                }).on('save', function (e, params) {
                    alert(params.newValue);
                    e.preventDefault();
                });
                
            },
            disabled: function () {
                $('#' + '<%= DataID %>').editable('disable', true);
            }
        };
       
        <% if(!this.Disabled) { %>
            editabletext.initialize();

        <% } else { %>
            
            editabletext.disabled();

        <% } %>
    });

</script>