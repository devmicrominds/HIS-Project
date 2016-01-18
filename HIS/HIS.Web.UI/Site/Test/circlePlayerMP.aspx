<%@ Page Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="true" CodeBehind="circlePlayerMP.aspx.cs" Inherits="HIS.Web.UI.Site.Test.circlePlayerMP" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            /*
             * Instance CirclePlayer inside jQuery doc ready
             *
             * CirclePlayer(jPlayerSelector, media, options)
             *   jPlayerSelector: String - The css selector of the jPlayer div.
             *   media: Object - The media object used in jPlayer("setMedia",media).
             *   options: Object - The jPlayer options.
             *
             * Multiple instances must set the cssSelectorAncestor in the jPlayer options. Defaults to "#cp_container_1" in CirclePlayer.
             */

            var myCirclePlayer = new CirclePlayer("#jquery_jplayer_1",
           {
               mp3: "http://kolber.github.io/audiojs/demos/mp3/01-dead-wrong-intro.mp3"

           }, {
               cssSelectorAncestor: "#cp_container_1"
           });

            // This code creates a 2nd instance. Delete if not required.

            var myOtherOne = new CirclePlayer("#jquery_jplayer_2",
            {
                m4a: "http://kolber.github.io/audiojs/demos/mp3/11-mo-stars-mo-problems.mp3"
                
            }, {
                cssSelectorAncestor: "#cp_container_2"
            });

        });
    </script>


    <div id="jquery_jplayer_1" class="cp-jplayer">
    </div>


    <div id="jquery_jplayer_2" class="cp-jplayer">
    </div>






    <div class="prototype-wrapper">
        <!-- A wrapper to emulate use in a webpage and center align -->


        <!-- The container for the interface can go where you want to display it. Show and hide it as you need. -->

        <div id="cp_container_1">
            <div class="cp-buffer-holder">

                <div class="cp-buffer-1"></div>
                <div class="cp-buffer-2"></div>
            </div>
            <div class="cp-progress-holder">
                <!-- .cp-gt50 only needed when progress is > than 50% -->
                <div class="cp-progress-1"></div>
                <div class="cp-progress-2"></div>
            </div>
            <div class="cp-circle-control"></div>
            <ul class="cp-controls">
                <li><a class="cp-play" tabindex="1">play</a></li>
                <li><a class="cp-pause" style="display: none;" tabindex="1">pause</a></li>
                <!-- Needs the inline style here, or jQuery.show() uses display:inline instead of display:block -->
            </ul>
        </div>
        </br>
                    </br>
                    </br>
                    </br>
                    </br>
    
                    <div id="cp_container_2">
                        <div class="cp-buffer-holder">
                            <div class="cp-buffer-1"></div>
                            <div class="cp-buffer-2"></div>
                        </div>
                        <div class="cp-progress-holder">
                            <div class="cp-progress-1"></div>
                            <div class="cp-progress-2"></div>
                        </div>
                        <div class="cp-circle-control"></div>
                        <ul class="cp-controls">
                            <li><a class="cp-play" tabindex="1">play</a></li>
                            <li><a class="cp-pause" style="display: none;" tabindex="1">pause</a></li>
                        </ul>
                    </div>


        <!-- This is the 2nd instance HTML -->



    </div>
</asp:Content>


