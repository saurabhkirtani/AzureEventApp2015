﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wac2015.Controls;
using Wac2015.Helpers;
using Wac2015.Models;
using Wac2015.ViewModels;
using Xamarin;
using Xamarin.Forms;

namespace Wac2015.Views
{

    public class ExtendedSessionFeedback : ContentPage
    {
        private readonly SessionViewModel _viewModel;
        private Entry registrationEntry;
        private long uuid = 0;
        private ITrackHandle handle;
        public ExtendedSessionFeedback()
        {
            //if (!Xamarin.Insights.IsInitialized)
            //{
            //    Xamarin.Insights.Initialize("cb9dddf47d18b81b88181a4f106fcb7565048148");
            //    Insights.ForceDataTransmission = true;
            //    if (!string.IsNullOrEmpty(App.uuid))
            //    {

            //        var manyInfos = new Dictionary<string, string> {
            //        { Xamarin.Insights.Traits.GuestIdentifier, App.uuid },
            //        { "CurrentCulture", CultureInfo.CurrentCulture.Name } 
            //    };

            //        Xamarin.Insights.Identify(App.uuid, manyInfos);
            //    }
            //}
            IDictionary<string, string> info = new Dictionary<string, string>();
            if (App.CurrentSession != null)
                info = new Dictionary<string, string> { { "SessionId", App.CurrentSession.Id } };
            handle = Insights.TrackTime("ExtendedFeedbackTime", info);

            var titleLabel = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "Session"
            };

            var title = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = Color.White,
                Text = App.CurrentSession.Title
            };

            var registrationIdLabel = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "Registration Id *"
            };

            registrationEntry = new Entry
            {
                Placeholder = "XXXXXXXX",
                TextColor = Device.OnPlatform(Color.Black, Color.White, Color.White),
                Keyboard = Keyboard.Numeric,

            };

            registrationEntry.Unfocused += async (sender, args) =>
            {
                if (!String.IsNullOrEmpty(registrationEntry.Text) && !long.TryParse(registrationEntry.Text, out uuid))
                {
                    registrationEntry.Text = "";
                    await DisplayAlert("Invalid Registration Id!", "Please check your Tag for Registration Id.", "OK");
                    registrationEntry.Focus();
                }

            };

            var changeLink = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Micro),
                  Font.SystemFontOfSize(14), Font.SystemFontOfSize(22)),
                FontAttributes = FontAttributes.Italic,
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                VerticalOptions = LayoutOptions.End,
                TextColor = App.XamDarkBlue,
                Text = "Edit"
            };

            var tap = new TapGestureRecognizer((view, obj) =>
            {
                registrationEntry.IsEnabled = true;
                registrationEntry.Focus();
                changeLink.IsVisible = false;
            });

            changeLink.GestureRecognizers.Add(tap);

            var registrationStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };

            if (!String.IsNullOrEmpty(App.uuid))
            {
                registrationEntry.Text = App.uuid;
                registrationEntry.IsEnabled = false;
                registrationStack.Children.Add(registrationEntry);
                registrationStack.Children.Add(changeLink);
            }
            else
            {
                registrationStack.Children.Add(registrationEntry);
                registrationEntry.IsEnabled = true;
            }

            var questionOne = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "Quality of Session Content *"
            };


            var answerOnePicker = new Picker
            {
                Title = "Select",
            };
            answerOnePicker.Items.Add("4 - Excellent");
            answerOnePicker.Items.Add("3");
            answerOnePicker.Items.Add("2");
            answerOnePicker.Items.Add("1 - Poor");

            var questionTwo = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "Quality of Session Delivery *"
            };

            var answerTwoPicker = new Picker
            {
                Title = "Select",
            };
            answerTwoPicker.Items.Add("4 - Excellent");
            answerTwoPicker.Items.Add("3");
            answerTwoPicker.Items.Add("2");
            answerTwoPicker.Items.Add("1 - Poor");

            var questionThree = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "Overall Session Quality *"
            };

            var answerThreePicker = new Picker
            {
                Title = "Select",
            };
            answerThreePicker.Items.Add("4 - Excellent");
            answerThreePicker.Items.Add("3");
            answerThreePicker.Items.Add("2");
            answerThreePicker.Items.Add("1 - Poor");

            var feedbackLabel = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "Feedback"
            };

            var txtFeedback = new Editor
            {
                VerticalOptions = LayoutOptions.FillAndExpand,

            };

            var extendedQuestion = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "Overall feedback for your experience at Azure Conference till now:"
            };

            var questionFour = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "How satisfied were you with this event *"
            };
            var answerFourPicker = new Picker
            {
                Title = "Select",
            };
            answerFourPicker.Items.Add("4 - Very Satisfied");
            answerFourPicker.Items.Add("3");
            answerFourPicker.Items.Add("2");
            answerFourPicker.Items.Add("1 - Very Dissatisfied");

            var questionFive = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "How likely are you to complete a web site, business solution, or consumer app (using any OS) that utilizes Azure services in the next 6 months? *"
            };
            var answerFivePicker = new Picker
            {
                Title = "Select",
            };
            answerFivePicker.Items.Add("4 - Very likely");
            answerFivePicker.Items.Add("3");
            answerFivePicker.Items.Add("2");
            answerFivePicker.Items.Add("1 - Not at all");

            var questionSix = new FontLabel
            {
                Font = Device.OnPlatform(Font.OfSize("HelveticaNeue-Light", NamedSize.Small),
                  Font.SystemFontOfSize(18), Font.SystemFontOfSize(22)),
                FontLabelType = FontLabelType.Light,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = App.XamBlue,
                Text = "How likely are you to recommend the use of Microsoft products/solutions to colleagues or peers? *"
            };
            var answerSixPicker = new Picker
            {
                Title = "Select",
            };
            answerSixPicker.Items.Add("4 - Very likely");
            answerSixPicker.Items.Add("3");
            answerSixPicker.Items.Add("2");
            answerSixPicker.Items.Add("1 - Not at all");

            var buttonReset = new Button
            {
                Text = "Reset"
            };

            var buttonSubmit = new Button
            {
                Text = "Submit"
            };

            buttonReset.Clicked += (sender, args) =>
            {
                answerOnePicker.SelectedIndex = answerTwoPicker.SelectedIndex = answerThreePicker.SelectedIndex =
                    answerFourPicker.SelectedIndex = answerFivePicker.SelectedIndex = answerSixPicker.SelectedIndex = -1;
                txtFeedback.Text = "";

            };

            var spinner = new ActivityIndicator
            {
                IsRunning = false,
                IsVisible = false,
                Color = Device.OnPlatform(App.XamBlue, Color.White, Color.White)
            };


            var isSubmitting = false;
            buttonSubmit.Clicked += async (sender, args) =>
            {
                if (!App.NetworkMonitor.IsAvailable())
                {
                    await DisplayAlert("No Internet Connectivity!", "Your Phone is not connected to the Internet. Please connect and try again.", "OK");
                }
                else if (isSubmitting == false)
                {
                    isSubmitting = true;
                    var answerOne = Utils.GetPickerValue(answerOnePicker.SelectedIndex);
                    var answerTwo = Utils.GetPickerValue(answerTwoPicker.SelectedIndex);
                    var answerThree = Utils.GetPickerValue(answerThreePicker.SelectedIndex);

                    var answerFour = Utils.GetPickerValue(answerFourPicker.SelectedIndex);
                    var answerFive = Utils.GetPickerValue(answerFivePicker.SelectedIndex);
                    var answerSix = Utils.GetPickerValue(answerSixPicker.SelectedIndex);

                    if (!String.IsNullOrEmpty(registrationEntry.Text) &&
                        !long.TryParse(registrationEntry.Text, out uuid))
                    {
                        registrationEntry.Text = "";
                        await DisplayAlert("Invalid Registration Id!", "Please check your Tag for Registration Id.", "OK");
                        registrationEntry.Focus();
                    }
                    else if (uuid == 0 || answerOne == -1 || answerTwo == -1 ||
                        answerThree == -1)
                    {
                        await
                              DisplayAlert("Mandatory inputs missing!",
                                  "Please provide your response for the mandatory(*) fields.", "OK");
                        isSubmitting = false;
                    }
                    else
                    {
                        try
                        {
                            spinner.IsRunning = true;
                            spinner.IsVisible = true;
                            var feedback = new Feedback2();
                            feedback.OverallRating = answerThree;
                            feedback.PlatformId = Device.OnPlatform(3, 2, 1);
                            feedback.SessionId = App.CurrentSession.Id;
                            feedback.SessionRating = answerOne;
                            feedback.SpeakerRating = answerTwo;
                            feedback.TextFeedback = txtFeedback.Text;
                            feedback.UserId = uuid;
                            App.uuid = uuid.ToString();
                            feedback.ToContact = true;

                            var eventFeedback = new Eventfeedback();
                            eventFeedback.UserId = uuid;
                            eventFeedback.OverallSatisfaction = answerFour;
                            eventFeedback.CompleteAzure = answerFive;
                            eventFeedback.RecommendAzure = answerSix;

                            Insights.Track("ContactUs Data", new Dictionary<string, string>{
                                {"UserID", uuid.ToString()},
                                {"Feedback", JsonConvert.SerializeObject(feedback)},
                                {"EventFeedback", JsonConvert.SerializeObject(eventFeedback)}
                            });

                            await
                                DisplayAlert("Submitting you feedback!", "Please wait while we submit your feedback.",
                                    "OK");
                            var res = await App.feedbackManager.SaveFeedbackTaskAsync(feedback);
                            var res1 = await App.feedbackManager.SaveEventFeedbackTaskAsync(eventFeedback);
                            if (!res || !res1)
                            {
                                await
                                    DisplayAlert("No Internet Connectivity!",
                                        "Your Phone is not connected to the Internet. Please connect and try again.",
                                        "OK");
                            }
                            else
                            {
                                App.FeedbackList.Add(App.CurrentSession.Id);
                                if (App.CurrentDayType == DayTypes.Day1)
                                {
                                    App.DayOneFeedbackCount = App.DayOneFeedbackCount + 1;
                                    App.DayOneExtFeedback = true;
                                    AppStorageHelper.SaveDayOneExtFeedback(1);
                                    AppStorageHelper.SaveDayOneFeedbackCount(App.DayOneFeedbackCount);
                                }
                                else if (App.CurrentDayType == DayTypes.Day2)
                                {
                                    App.DayTwoFeedbackCount = App.DayTwoFeedbackCount + 1;
                                    App.DayTwoExtFeedback = true;
                                    AppStorageHelper.SaveDayTwoExtFeedback(1);
                                    AppStorageHelper.SaveDayTwoFeedbackCount(App.DayTwoFeedbackCount);
                                }
                                await DisplayAlert("Thank You!", "Thanks for providing your valuable feedback.", "OK");
                                handle.Stop();
                                await this.Navigation.PopAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            Insights.Report(ex);
                        }
                        isSubmitting = false;
                        spinner.IsVisible = false;
                        spinner.IsRunning = false;
                    }
                    
                }
            };

            var stackButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { buttonReset, buttonSubmit },
            };

            var feedbackStack = new StackLayout
            {
                Children = { titleLabel, title, registrationIdLabel, registrationStack, questionOne, answerOnePicker, 
                    questionTwo, answerTwoPicker, questionThree, answerThreePicker, feedbackLabel, txtFeedback, 
                    extendedQuestion, questionFour, answerFourPicker, questionFive, answerFivePicker, questionSix, 
                    answerSixPicker, spinner, stackButtons },
            };

            var scrollView = new ScrollView
            {
                VerticalOptions = LayoutOptions.Fill,
                Orientation = ScrollOrientation.Vertical,
                Content = feedbackStack,

            };

            Content = scrollView;
            Title = "Feedback";
            Padding = new Thickness(10, 0);
            BackgroundColor = Color.Black;

            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (String.IsNullOrEmpty(App.uuid))
            {
                registrationEntry.Focus();
            }
        }
    }
}