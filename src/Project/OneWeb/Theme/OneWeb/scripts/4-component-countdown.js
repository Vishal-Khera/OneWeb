
let now = new Date();
let prgressvalue = 100 / 60
let eventText =  $('#event-text').val();

function updateTimer(time, basecolor, roundcolor,element) {
    future = Date.parse(time);
    now = new Date();
    diff = future - now;
    days = Math.floor(diff / (1000 * 60 * 60 * 24));
    hours = Math.floor(diff / (1000 * 60 * 60));
    mins = Math.floor(diff / (1000 * 60));
    secs = Math.floor(diff / 1000);
    d = days;
    h = hours - days * 24;
    m = mins - hours * 60;
    s = secs - mins * 60;
    if (diff < 0) {
        $('div[timer=days]').map(function () {
            if ($(element).hasClass('circle')) {
                $(this).text(eventText);
            }
            $(this).text(eventText);
        })
        $('div[timer=Hrs]').map(function () {
            if ($(element).hasClass('circle')) {
                $(this).text();
            }
            $(this).text();
        })
        $('div[timer=minutes]').map(function () {
            if ($(element).hasClass('circle')) {
                $(this).text();
            }
            $(this).text();
        })
        $('div[timer=Seconds]').map(function () {
            if ($(element).hasClass('circle')) {
                $(this).text();
            }
            $(this).text();
        })
        clearInterval(updateTimer);
        return false
    }

    $('div[timer=days]').map(function () {
        if ($(element).hasClass('circle')) {
            $(this).text(d + " Days");
        }
        $(this).text(d + " Days");
    })
    $('div[timer=Hrs]').map(function () {
        if ($(element).hasClass('circle')) {
            $(this).text(h + " Hrs. ");
        }
        $(this).text(h + " Hrs. ");
    })
    $('div[timer=minutes]').map(function () {
        if ($(element).hasClass('circle')) {
            $(this).text(m + " Min. ");
        }
        $(this).text(m + " Min.")
    })
    $('div[timer=Seconds]').map(function () {
        if ($(element).hasClass('circle')) {
            $(this).text(s + " Sec. ");
        }
        $(this).text(s + " Sec. ")
    })
}
    let basecolor = ""
    let roundcolor  = ""
setInterval(function () {
    
    $(".timercountdown").each(function (event) {
        element = this
        time = this.getAttribute('time')
        basecolor = this.getAttribute("animatedcolor")
        roundcolor =  this.getAttribute("basecolor")
        updateTimer(time, basecolor, roundcolor,element)
    });
    
    
}, 1000);


