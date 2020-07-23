
(function ($) {
    jQuery.validator.addMethod("ValidateCreditCardNumber", function (value, element) {
        var ccNum = value;
        var visaRegEx = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
        var mastercardRegEx = /^(?:5[1-5][0-9]{14})$/;
        var amexpRegEx = /^(?:3[47][0-9]{13})$/;
        var discovRegEx = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
        var isValid = false;

        if (visaRegEx.test(ccNum)) {
            $('.addStyle i').removeClass().addClass("cards-billing fab fa-cc-visa");
            isValid = true;
        } else if (mastercardRegEx.test(ccNum)) {
            $('.addStyle i').removeClass().addClass("cards-billing fab fa-cc-mastercard");
            isValid = true;
        } else if (amexpRegEx.test(ccNum)) {
            $('.addStyle i').removeClass().addClass("cards-billing fab fa-cc-amex");
            isValid = true;
        } else if (discovRegEx.test(ccNum)) {
            $('.addStyle i').removeClass().addClass("cards-billing fa-cc-discover");
            isValid = true;
        }

        if (isValid) {
            return true;
        } else {
            $('.addStyle i').removeClass();
            return false;
        }
    }, 'Please enter a valid Credit Card.');
    jQuery.validator.addMethod("ValidateAmount", function (value, element) {
        if (value == "0.00$" || value == "0$") {
            return false;
        } else {
            return true;
        }
    }, 'Please select one service');
    var form = $("#signup-form");
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
             element.before(error); 
        },rules: {
            
            Social_security_number: {
                required: true,
            },
            Social_security_number_confirm: {
                equalTo: "#Social_security_number"
            },
            CreditCardNumber: {
                required: true,
                ValidateCreditCardNumber: true
            },
            Amount: {
                required: true,
                ValidateAmount: true
            }
        },
        messages: {
            First_name: {
                required: "Enter your First Name"
            },
            Last_name: {
                required: "Enter your Last Name"
            },
            Email: {
                required: "Enter your Email Address"
            },
            Address: {
                required: "Enter your Street Address & Unit"
            },
            City: {
                required: "Enter your City"
            },
            State: {
                required: "Select your State"
            },
            Zip: {
                required: "Enter your Zip Code"
            },
            Phone: {
                required: "Enter your Phone Number"
            },
            CreditCardNumber: {
                required: "Enter your Credit Card Number"
            },
            Month: {
                required: "Enter Month"
            },
            Year: {
                required: "Enter Year"
            },
            Cvc: {
                required: "Enter your Security Code"
            }
      
        },
        focusInvalid: false,
        invalidHandler: function (form, validator) {

            if (!validator.numberOfInvalids())
                return;

            $('html, body').animate({
                scrollTop: $(validator.errorList[0].element).offset().top
            }, 800);

        },
        onfocusout: function(element) {
            $(element).valid();
        },
        highlight : function(element, errorClass, validClass) {
            $(element.form).find('.actions').addClass('form-error');
            $(element).removeClass('valid');
            $(element).addClass('error');
        },
        unhighlight: function(element, errorClass, validClass) {
            $(element.form).find('.actions').removeClass('form-error');
            $(element).removeClass('error');
            $(element).addClass('valid');
        }
    });
    form.steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "fade",
        labels: {
            previous : 'Previous',
            next : 'Next',
            finish : 'Submit',
            current : ''
        },
        titleTemplate : '<span class="title">#title#</span>',
        onStepChanging: function (event, currentIndex, newIndex)
        {
            var form2 = document.getElementById("signup-form");
            var data = toJSONString(form2), obj = jQuery.parseJSON(data), list_services = [], html = "";
            var amounts = 0;
            $('#fullname-val').text(obj.First_name + ' ' + obj.Last_name);
            $('#email-val').text(obj.Email);
            $('#address-val').text(obj.Address);
            $('#city-val').text(obj.City);
            $('#state-val').text(obj.State);
            $('#zip-val').text(obj.Zip);
            $('#phone-val').text(obj.Phone);
            $('#pay-val').text(getTypePay(obj.CreditCardNumber));
            $(".checkServices").each(function (index, element) {
                $me = $(this);
                if ($me.is(':checked')) {
                    list_services.push({ name: $me.attr("name"), value: $me.val() });
                }
            });
            for (var item of list_services) {
                html += "<tr><th>" + item.name + "</th><td>" + item.value + "$</td></tr>";
                amounts +=  parseFloat(item.value);
            }
            html += "<tr class='tb-total'><th>Total</th><td>" + amounts + "$</td></tr>";
            $("#tb-services").html(html);
           
            form.validate().settings.ignore = ":disabled,:hidden";
            return form.valid();
        },
        onFinishing: function (event, currentIndex)
        {
            form.validate().settings.ignore = ":disabled";
            return form.valid();
        },
        onFinished: function (event, currentIndex)
        {   
            $("#signup-form").submit(function (event) {
                event.preventDefault(); //prevent default action 
                var post_url = $(this).attr("action"); //get form action url
                var request_method = $(this).attr("method"); //get form GET/POST method
                var form_data = $(this).serialize(); //Encode form elements for submission
                $.ajax({
                    url: post_url,
                    type: request_method,
                    data: form_data
                }).done(function (response) { 
                    swal({
                        title: "Gracias de parte de CISEP " + response.clientName,
                        text: "Puedes optar por mas servicios en nuestro portal de inicio.",
                        icon: "success",
                        dangerMode: true,
                    })
                        .then(willDelete => {
                            if (willDelete) {
                                window.location.href = "/Home/Index";
                            }
                        });
                });
            });
            form.submit();                     
        },
    });

    jQuery.extend(jQuery.validator.messages, {
        required: "",
        remote: "",
        email: "",
        url: "",
        date: "",
        dateISO: "",
        number: "",
        digits: "",
        creditcard: "",
        equalTo: ""
    });
   
      
    function toJSONString(form2) {
        var obj = {};
        var elements = form2.querySelectorAll("input, select, textarea");
        for (var i = 0; i < elements.length; ++i) {
            var element = elements[i];
            var name = element.name;
            var value = element.value;

            if (name) {
                obj[name] = value;
            }
        }
        return JSON.stringify(obj);
    }
    function getTypePay(value) {
        var ccNum = value;
        var visaRegEx = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
        var mastercardRegEx = /^(?:5[1-5][0-9]{14})$/;
        var amexpRegEx = /^(?:3[47][0-9]{13})$/;
        var discovRegEx = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
        var isValid = false;

        if (visaRegEx.test(ccNum)) {
            return "Visa";
        } else if (mastercardRegEx.test(ccNum)) {
            return "MasterCard";
        } else if (amexpRegEx.test(ccNum)) {
            return "American Express";
        } else if (discovRegEx.test(ccNum)) {
            return "Discover";
        }
    }

    
})(jQuery);