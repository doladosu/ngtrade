$(function() {
    $("a[class=redirectLink]").each(function () {
        var encodedUrl = encodeURIComponent($(this).attr('href'));
        var href = '/home/newsdetail?surl=' + encodedUrl;
        $(this).attr("href", href);
  });
  //var topic = getParameterByName('surl');
  //$('#topicTitle').append(topic);
});

function getParameterByName(name) {
  name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
  var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
      results = regex.exec(location.search);
  return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}