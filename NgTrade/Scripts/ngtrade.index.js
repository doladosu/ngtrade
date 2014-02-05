/*
 * Copyright 2012 soundarapandian
 * Licensed under the Apache License, Version 2.0
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

$("document").ready(function() {

    var isLoggedIn = $.cookie("lg");
    if (isLoggedIn != undefined && isLoggedIn == "1") {
        $('[data-loggedin="true"]').show();
    } else {
        $('[data-loggedin="false"]').show();
    }
    /*Start: Prevent the default white background on blur of top navigation */
	$(".dropdown-menu li a").mousedown(function() {
		var dropdown = $(this).parents('.dropdown');
		var link = dropdown.children(':first-child');
		link.css('background-color', "#2E3436");
		link.css('color', 'white');
	});
	/*End: Prevent the default white background on blur of top navigation */

  /*Start : Automatically start the slider */
	$('.carousel').carousel({
      interval: 4000
   });
    /*End: Automatically start the slider */
    
	$.getJSON('/Home/GetSymbols', function (data) {
	    $.each(data, function (key, val) {
	        $("#symbols").append('<option>' + val + '</option>');
	    });
	
	});
	$("#btnSubscribe").on("click", function () {
	    var email = $("#subscribe").val();
	    if (email != '') {
	        $.getJSON('/Home/AddEmailToNewsletter?email=' + email, function (data) {
	            if (data != null) {
	                alert(data);
	            } else {
	                alert('Error occured, please try again');
	            }
	        });
	    }
	});
});
