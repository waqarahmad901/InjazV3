/*****
* CONFIGURATION
*/
    //Main navigation
    $.navigation = $('nav > ul.nav');

  $.panelIconOpened = 'icon-arrow-up';
  $.panelIconClosed = 'icon-arrow-down';

  //Default colours
  $.brandPrimary =  '#20a8d8';
  $.brandSuccess =  '#4dbd74';
  $.brandInfo =     '#63c2de';
  $.brandWarning =  '#f8cb00';
  $.brandDanger =   '#f86c6b';

  $.grayDark =      '#2a2c36';
  $.gray =          '#55595c';
  $.grayLight =     '#818a91';
  $.grayLighter =   '#d1d4d7';
  $.grayLightest =  '#f8f9fa';

'use strict';

/****
* MAIN NAVIGATION
*/

$(document).ready(function($){

  // Add class .active to current link
  $.navigation.find('a').each(function(){

    var cUrl = String(window.location).split('?')[0];

    if (cUrl.substr(cUrl.length - 1) == '#') {
      cUrl = cUrl.slice(0,-1);
    }

    if ($($(this))[0].href==cUrl) {
      $(this).addClass('active');

      $(this).parents('ul').add(this).each(function(){
        $(this).parent().addClass('open');
      });
    }

    //var countryData = $.fn.intlTelInput.getCountryData();  
    //  $.each(countryData, function (i, country) {
    //      console.log("insert into lk_country values('" + country.name + "','" + country.iso2 + "')");
    //  });

      
  });
  var telInput = $(".phone");

    // initialise plugin
  telInput.intlTelInput({
      initialCountry: "sa",
     // utilsScript: "/Scripts/utils.js",
      separateDialCode: true
  });


  $("form").unbind('submit').submit(function (e) {
      if ($(this).valid()) {
          debugger;
          if ($(".voldate").val() != undefined && $(".voldate").val() != "")
          {
              var val = $(".voldate").val();
              var dateOfBirth = val;
              var arr_dateText = dateOfBirth.split("/");
              day = arr_dateText[0];
              month = arr_dateText[1];
              year = arr_dateText[2];

              var mydate = new Date();
              mydate.setFullYear(year, month - 1, day);

              var maxDate = new Date();
              maxDate.setFullYear(maxDate.getFullYear() - 10);

              if (maxDate < mydate) {
                  
                  $(".voldate").next().html("<span for='DateOfBirth' class=''>Sorry, only persons over the age of 10 can be covered</span>")
                  return false;
              }
              return true;
             
          }
          if ($(".coorddocument").val() == "")
          {
              alert("Please upload document");
              return false;
          }
          for (var i = 0; i < $(".phone").length; i++) {
              var phoneNo = $($(".phone")[i]).intlTelInput("getNumber");
              if(phoneNo != "")
              $($(".phone")[i]).val(phoneNo);
          };

          $("button[type=submit][clicked=true]").button('loading');
      } else
          return false;
    
  });
    bindDates();

  //$(".date").datepicker({
  //    format: 'dd/mm/yyyy',
  //    todayHighlight: true
  //}).on('changeDate', function (e) {
  //    $(this).datepicker('hide');
  //}); 
     // By pass date validation. 
  $.validator.addMethod('date',
  function (value, element) {
      if (this.optional(element)) {
          return true;
      } 
      var ok = true;
       
      return ok;
  });
 
  // Dropdown Menu
  $.navigation.on('click', 'a', function(e){

    if ($.ajaxLoad) {
      e.preventDefault();
    }

    if ($(this).hasClass('nav-dropdown-toggle')) {
      $(this).parent().toggleClass('open');
      resizeBroadcast();
    }

  });

  function resizeBroadcast() {

    var timesRun = 0;
    var interval = setInterval(function(){
      timesRun += 1;
      if(timesRun === 5){
        clearInterval(interval);
      }
      window.dispatchEvent(new Event('resize'));
    }, 62.5);
  }

  /* ---------- Main Menu Open/Close, Min/Full ---------- */
  $('.navbar-toggler').click(function(){

    if ($(this).hasClass('sidebar-toggler')) {
      $('body').toggleClass('sidebar-hidden');
      resizeBroadcast();
    }

    if ($(this).hasClass('sidebar-minimizer')) {
      $('body').toggleClass('sidebar-compact');
      resizeBroadcast();
    }

    if ($(this).hasClass('aside-menu-toggler')) {
      $('body').toggleClass('aside-menu-hidden');
      resizeBroadcast();
    }

    if ($(this).hasClass('mobile-sidebar-toggler')) {
      $('body').toggleClass('sidebar-mobile-show');
      resizeBroadcast();
    }

  });

  $('.sidebar-close').click(function(){
    $('body').toggleClass('sidebar-opened').parent().toggleClass('sidebar-opened');
  });

  /* ---------- Disable moving to top ---------- */
  $('a[href="#"][data-top!=true]').click(function(e){
    e.preventDefault();
  });
  $("input:text,form").attr("autocomplete", "kuchbee");


  $('button[type="submit"]').each(function (index, ele) {
      var $this = $(this);
      var value = "<i class='fa fa-dot-circle-o'></i>"+$this.text() +" <i class='fa fa-spinner fa-spin'></i>";
     
      $this.attr('data-loading-text', value);
  })

});


$("form button[type=submit]").click(function () {
    $("button[type=submit]", $(this).parents("form")).removeAttr("clicked");
    $(this).attr("clicked", "true");
});

    function bindDates() {
        $('.date-islamic').calendarsPicker({
            calendar: $.calendars.instance('islamic'),
            dateFormat: 'yyyy/mm/dd',
            onSelect: function (dates) { 
              
                var dateselect = dates[0]._day + "-" + dates[0]._month + "-" + dates[0]._year;
                var control = $(this);
                $.get("/Home/ConvertDateCalendar?dateConv=" + dateselect + "&calendar=Gregorian&dateLangCulture=en-us", function (res) {
                    control.parent().parent().find('.date').val(res);
                    if ($(control).hasClass("startDateIslamic") || $(control).hasClass("enddateIslamic")) {
                        renderTimeControl && renderTimeControl();
                    }
                });
            }
        });

        $('.date').calendarsPicker({
            calendar: $.calendars.instance('gregorian'),
            dateFormat: 'dd/mm/yyyy',
            onSelect: function (dates) { 
                if ($(this).hasClass("startDate") || $(this).hasClass("enddate"))
                {
                    renderTimeControl && renderTimeControl(); 
                }
                var dateselect = dates[0]._day + "-" + dates[0]._month + "-" + dates[0]._year;
                var control = $(this);
                $.get("/Home/ConvertDateCalendar?dateConv=" + dateselect + "&calendar=Hijri&dateLangCulture=en-us", function (res) {
                    control.parent().parent().find('.date-islamic').val(res);
                });

            }
        });

    }
/****
* CARDS ACTIONS
*/

$(document).on('click', '.card-actions a', function(e){
  e.preventDefault();

  if ($(this).hasClass('btn-close')) {
    $(this).parent().parent().parent().fadeOut();
  } else if ($(this).hasClass('btn-minimize')) {
    var $target = $(this).parent().parent().next('.card-block');
    if (!$(this).hasClass('collapsed')) {
      $('i',$(this)).removeClass($.panelIconOpened).addClass($.panelIconClosed);
    } else {
      $('i',$(this)).removeClass($.panelIconClosed).addClass($.panelIconOpened);
    }

  } else if ($(this).hasClass('btn-setting')) {
    $('#myModal').modal('show');
  }

});

function capitalizeFirstLetter(string) {
  return string.charAt(0).toUpperCase() + string.slice(1);
}

function init(url) {

  /* ---------- Tooltip ---------- */
  $('[rel="tooltip"],[data-rel="tooltip"]').tooltip({"placement":"bottom",delay: { show: 400, hide: 200 }});

  /* ---------- Popover ---------- */
  $('[rel="popover"],[data-rel="popover"],[data-toggle="popover"]').popover();

}

$(".comment").click(function(){
    $(this).next(".comment-box").toggle();
});

function ValidateSingleInput(oInput, _validFileExtensions) {
    if (oInput.type == "file") {
        var sFileName = oInput.value;
        if (sFileName.length > 0) {
            var blnValid = false;
            for (var j = 0; j < _validFileExtensions.length; j++) {
                var sCurExtension = _validFileExtensions[j];
                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                    blnValid = true;
                    break;
                }
            }

            if (!blnValid) {
                alert("Sorry, " + sFileName + " is invalid, allowed extensions are: " + _validFileExtensions.join(", "));
                oInput.value = "";
                return false;
            }
        }
    }
    return true;
}


function PopupCenter(url, title, w, h) {
    // Fixes dual-screen position                         Most browsers      Firefox
    var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
    var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

    var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
    var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

    var left = ((width / 2) - (w / 2)) + dualScreenLeft;
    var top = ((height / 2) - (h / 2)) + dualScreenTop;
    var newWindow = window.open(url, title, 'scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

    // Puts focus on the newWindow
    if (window.focus) {
        newWindow.focus();
    }
}


function makeTable(container, data) {
   
    var table = $("<table/>").addClass('table table-hover table-outline mb-0');
    $.each(data, function (rowIndex, r) {
        var row = $("<tr/>");
        $.each(r, function (colIndex, c) {
            var w = (100 / r.length) + '%';
            row.append($("<t" + (rowIndex == 0 ? "h" : "d") + " style='width: "+ w +"'/>").text(c));
        });
        table.append(row);
    });
    return $("#table").append(table);
}

function renderTimeControl() {
    debugger;
    var startDate = $(".startDate").val();
    var endDate = $(".enddate").val();
    arr_dateText = startDate.split("/");
    day = arr_dateText[0];
    month = arr_dateText[1];
    year = arr_dateText[2];

    var dateStart = new Date();
    dateStart.setFullYear(year, month - 1, day);

    arr_dateText = endDate.split("/");

    day = arr_dateText[0];
    month = arr_dateText[1];
    year = arr_dateText[2];

    var dateEnd = new Date();
    dateEnd.setFullYear(year, month - 1, day);

    if (dateStart > dateEnd) {
        alert("Date is not set propely");
    }

    var dateDiff = date_diff_indays(dateStart, dateEnd);

    $("#timeControl > div").not(".firstRow").remove();

    var monthNames = ["January", "February", "March", "April", "May", "June",
                     "July", "August", "September", "October", "November", "December"
    ];
    var newDate = dateStart;
    for (i = 0; i <= dateDiff; i++) {
        var html = "<div class='form-group col-sm-12'>";
        html += "  <div class='col-sm-2' style='padding:0px'>"
        html += "<input type='checkbox' name = 'procheck_" + i + "' onchange = 'changeTimeCheck(this,\"proFromTime_\",\"proToTime_\"," + i + ")'/>"
        html += "</div>"
        html += "  <div class='col-sm-2' style='padding:0px'>"
        html += newDate.getDate() + " " + monthNames[newDate.getMonth()]
        html += "</div>"
        html += "<div class='col-sm-4'>"
        html += "<input type='text' name='proFromTime_" + i + "' class='form-control timepicker' data-provide='timepicker'/>"
        html += "</div>"
        html += "<div class='col-sm-4'>"
        html += "<input type='text' class='form-control timepicker' name='proToTime_" + i + "' data-provide='timepicker'/>"
        html += " </div> "
        html += " </div> "

        $("#timeControl").append(html);

        newDate.setDate(dateStart.getDate() + 1);
    }
    bindTimePicker();

}

function bindTimePicker()
{
    $('.timepicker').timepicker({
        timeFormat: 'hh:mm p',
        interval: 15,
        minTime: '10',
        maxTime: '6:00pm', 
        startTime: '10:00',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
}

var date_diff_indays = function (date1, date2) {
    dt1 = new Date(date1);
    dt2 = new Date(date2);
    return Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())) / (1000 * 60 * 60 * 24));
}


function changeTimeCheck(checkbox,from,to, index)
{
    debugger;
    if (checkbox.checked == true) {
        $('input[name*="' + from + index + '"]').attr('readonly', 'readonly');
        $('input[name*="' + to + index + '"]').attr('readonly', 'readonly');
    } else {
        $('input[name*="' + from + index + '"]').removeAttr('readonly');
        $('input[name*="' + to + index + '"]').removeAttr('readonly');
    }
}
 


