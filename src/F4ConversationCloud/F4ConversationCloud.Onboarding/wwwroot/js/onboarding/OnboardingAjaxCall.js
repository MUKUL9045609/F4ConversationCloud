
// varify mobile number
$(document).ready(function () {
    $('#PhoneNumber').on('input', function () {
        let number = $(this).val().trim();

        if (number === '') {
            $('#phoneError').text('Please enter your Phone Number.').show();
        } else if (!/^\d{10}$/.test(number)) {
            $('#phoneError').text('Please enter a valid 10-digit phone number.').show();
        } else {
            $('#phoneError').text('').hide();
        }
    });
    $('#varifyPhoneNumber').click(function () {
        let number = $('#PhoneNumber').val().trim();

        if (number === '') {
            $('#phoneError').text('Please enter your Phone Number.').show();
            return;

        }
        else {
            $('#phoneError').text('').hide();
        }
        $("#loader").show();
        $.ajax({
            url: "@Url.ActionLink("VarifyMobileNo", "Onboarding")",
            type: "POST",
            data: JSON.stringify({
                UserPhoneNumber: number,
                OTP_Source: "PhoneNumber"
            }),
            contentType: "application/json; charset=utf-8",
            success: function (response) {

                if (response.status === true) {
                    // alert("OTP sent successfully!");
                    showToast("OTP sent successfully!", "success");
                    $('#varifyPhoneNumber').hide();
                    $('#codeSection').show()
                }
                else if (response.status === false && response.message === "Phone Number already exists") {
                    $('#phoneError').text('Phone Number already exists. Please use a different Phone Number.').show();
                } else {
                    //  alert("Failed to send OTP. Please try again.");
                    showToast("Failed to send OTP. Please try again!", "danger");
                }
            },
            error: function (xhr) {
                showToast("Failed to send OTP. Please try again!", "danger");
            },
            complete: function () {
                $("#loader").hide();
            }

        });
    });


});

// resend otp
$(document).ready(function () {
    let timerDuration = 1 * 60;
    let interval;

    function startTimer() {
        clearInterval(interval);

        let remaining = timerDuration;

        $("#resetBtn").addClass("d-none");
        $(".otp-text").removeClass("d-none");
        $("#timeRemaining").text(formatTime(remaining));

        interval = setInterval(function () {
            remaining--;

            if (remaining >= 0) {
                $("#timeRemaining").text(formatTime(remaining));
            }

            if (remaining < 0) {
                clearInterval(interval);
                $(".otp-text").addClass("d-none");
                $("#resetBtn").removeClass("d-none");
            }
        }, 1000);
    }

    function formatTime(seconds) {
        let minutes = Math.floor(seconds / 60);
        let secs = seconds % 60;
        return `${minutes}:${secs < 10 ? "0" + secs : secs}`;
    }


    $("#varifyPhoneNumber").click(function () {
        startTimer();
    });


    $("#resetBtn").click(function () {

        //  otpVerified = false;

        let number = $("#PhoneNumber").val().trim();
        $("#loader").show();

        $.ajax({
            url: "@Url.Action("VarifyMobileNo", "Onboarding")",
            type: "POST",
            data: JSON.stringify({
                UserPhoneNumber: number,
                OTP_Source: "PhoneNumber"
            }),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.status === true) {
                    showToast("OTP sent successfully!", "success");
                    startTimer();
                }
                else {
                    showToast("Failed to resend OTP. Please try again!", "danger");
                }
            },
            error: function (xhr) {
                showToast("Failed to resend OTP. Please try again!", "danger");
            },
            complete: function () {
                $("#loader").hide();
            }
        });

    });
});


// verify otp
$(document).ready(function () {

    $('#varifyOtp').click(function () {
        let Otp = $('#otp').val().trim();
        let number = $('#PhoneNumber').val().trim();

        if (Otp === '') {
            $('#OtpError').text('Please enter the OTP.');
            return;
        } else {
            $('#OtpError').text('');
        }

        $("#loader").show();
        $.ajax({
            url: "@Url.ActionLink("VerifyOtp", "Onboarding")",
            type: "POST",
            data: JSON.stringify({
                UserPhoneNumber: number,
                OTP: Otp,
                OTP_Source: "PhoneNumber"
            }),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                $('#PhoneOtpVerified').val(response.status);
                if (response.status === true) {
                    showToast("OTP verified successfully!", "success");
                    $('#OtpError').text('');
                    $('#codeSection').hide();
                    $('#PhoneOtpVerifiederror').hide();
                    $('#varifyPhoneNumber').addClass('disabled-btn');
                    $('#varifyPhoneNumber span').text('');

                    $('#PhoneNumber')
                        .prop('readonly', true)
                        .removeClass('is-invalid')
                        .addClass('form-control flex-grow-1 success');


                } else {
                    $('#OtpError').text('Invalid OTP. Please Enter Valid OTP.');
                    otpVerified = false;
                }
            },
            error: function () {
                $('#OtpError').text('Failed to verify OTP. Please try again.');
                otpVerified = false;
            },
            complete: function () {
                $("#loader").hide();
            }
        });
    });

});



// #region Resend otp-- >

$(document).ready(function () {
                     let timerDuration = 5 * 60;
                     let interval;

                     function startTimer() {
                         clearInterval(interval);

                         let remaining = timerDuration;

                         $("#resetBtn").addClass("d-none");
                         $(".otp-text").removeClass("d-none");
                         $("#timeRemaining").text(formatTime(remaining));

                         interval = setInterval(function () {
                             remaining--;

                             if (remaining >= 0) {
                                 $("#timeRemaining").text(formatTime(remaining));
                             }

                             if (remaining < 0) {
                                 clearInterval(interval);
                                 $(".otp-text").addClass("d-none");
                                 $("#resetBtn").removeClass("d-none");
                             }
                         }, 1000);
                     }

                     function formatTime(seconds) {
                         let minutes = Math.floor(seconds / 60);
                         let secs = seconds % 60;
                         return `${minutes}:${secs < 10 ? "0" + secs : secs}`;
                     }


                     $("#sendmail").click(function () {
                         startTimer();
                     });


                     $("#resetBtn").click(function () {

                       otpVerified = false;

                        let email = $('#emailid').val().trim();
                          $("#loader").show();

                       $.ajax({
                               url: "@Url.ActionLink("VarifyMail", "Onboarding")",
                                type: "POST",
                                data: JSON.stringify({
                                    UserEmailId: email,
                                    OTP_Source: "EMAIL"
                                }),
                                contentType: "application/json; charset=utf-8",
                                success: function (response) {
                                    if (response.status === true) {
                                         $('#codeSection').show();
                                       // alert("OTP sent successfully!");
                                        showToast("OTP sent successfully!", "success");
                                    }
                                    else if(response.status === false && response.message === "Email already exists")
                                    {
                                        $('#emailError').text('Email already exists. Please use a different email.');
                                    }
                                    else {
                                        showToast("Failed to send OTP. Please try again.", "danger");
                                    }

                                },
                                error: function (xhr) {
                                   showToast("technical Error", "danger");
                                },
                                    complete: function () {
                                  $("#loader").hide();
                                }
                       });



                     });
                 });

    
//#region send mail --

 $(document).ready(function () {


            $('#sendmail').click(function(){
                let email = $('#emailid').val().trim();

                if(email === ''){
                   // $('#sendmail').prop('disabled', true);
                    $('#emailError').text('Please enter your email address.');
                    return;
                } else {
                    $('#emailError').text('');
                        //    $('#sendmail').prop('disabled', false);
                }

                let emailPattern = /^[^\s@@]+@@[^\s@@]+\.[^\s@@]+$/;

                if (!emailPattern.test(email)) {
                    $('#emailError').text('Please enter a valid email address');

                    return;
                }
                $("#loader").show();
                $.ajax({
                    url:"@Url.ActionLink("VarifyMail", "Onboarding")",
                    type: "POST",
                    data: JSON.stringify({
                        UserEmailId: email,
                        //UserPhoneNumber: $('#phone').val(),
                        OTP_Source: "EMAIL"
                    }),
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response.status === true) {
                             $('#codeSection').show();
                             showToast("OTP sent successfully!", "success");
                             $('#sendmail').addClass('disabled-btn');
                             $('#sendmail span').text('');
                        }
                        else if(response.status === false && response.message === "Email already exists")
                        {
                            $('#emailError').text('Email already exists. Please use a different email.');

                             showToast("Email already exists. Please use another one.", "warning");
                        }
                        else {
                                    showToast("Failed to send OTP. Please try again.", "danger");
                        }

                    },
                    error: function (xhr) {
                                        showToast("technical Error", "danger");
                    },
                    complete: function () {
                        $("#loader").hide();
                    }
                });

            });

        });

// varify mail otp
$(document).ready(function () {
    
     $('#varifyOtp').click(function () {
            let Otp = $('#otp').val();
                    let email = $('#emailid').val().trim();

            if (Otp === '' || email === '') {
                $('#OtpError').text('Please enter the OTP and email.');
                otpVerified = false;
                return;
            }
                     $("#loader").show();
            $.ajax({
                url:"@Url.ActionLink("VerifyOtp", "Onboarding")",
                type: "POST",
                data: JSON.stringify({
                    UserEmailId: email,
                    OTP: Otp,
                    OTP_Source: "EMAIL"
                }),
                contentType: "application/json; charset=utf-8",
                success: function (response) {

                    $('#OTP').val(response.status);

                    if (response.status === true) {
                        
                       showToast("OTP verified successfully!", "success");
                        $('#OtpError').text('');
                        $('#codeSection').hide();
                        $('#sendmail').addClass('disabled-btn');
                        $('#sendmail span').text('');
                        $('#emailvarifyerror').hide();

                                $('#emailid')
                        .prop('readonly', true)
                        .removeClass('is-invalid')
                        .addClass('form-control flex-grow-1 success');
                    } else {
                        $('#OtpError').text('Invalid OTP. Please try again.');
                        
                    }
                },
                error: function (xhr) {
                    $('#OtpError').text('Failed to verify OTP. Please try again.');
               
                },
                 complete: function () {
                    
                        $("#loader").hide();
               }
            });
        });
               
});

