using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using RestSharp;
using static SlackPurge.Message;
using static SlackPurge.RootObject;
using System.Threading;

namespace SlackPurge
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            TokenEntry.Completed += (object sender, EventArgs e) =>
            {
                ChannelEntry.Focus();
            };

            string tokenProperty, channelProperty, userProperty;
            if (App.Current.Properties.ContainsKey("tokenProperty"))
            {
                tokenProperty = App.Current.Properties["tokenProperty"] as string;
                TokenEntry.Text = tokenProperty;
            }
            if (App.Current.Properties.ContainsKey("channelProperty"))
            {
                channelProperty = App.Current.Properties["channelProperty"] as string;
                ChannelEntry.Text = channelProperty;
            }
            if (App.Current.Properties.ContainsKey("userProperty"))
            {
                userProperty = App.Current.Properties["userProperty"] as string;
                UserEntry.Text = userProperty;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            if (App.Current.Properties.ContainsKey("tokenProperty"))
            {
                App.Current.Properties["tokenProperty"] = TokenEntry.Text;
                //App.Current.SavePropertiesAsync();
            }
            else
            {
                App.Current.Properties.Add("tokenProperty", TokenEntry.Text);
            }
            if (App.Current.Properties.ContainsKey("channelProperty"))
            {
                App.Current.Properties["channelProperty"] = ChannelEntry.Text;
            }
            else
            {
                App.Current.Properties.Add("channelProperty", ChannelEntry.Text);
            }
            if (App.Current.Properties.ContainsKey("userProperty"))
            {
                App.Current.Properties["userProperty"] = UserEntry.Text;
            }
            else
            {
                App.Current.Properties.Add("userProperty", UserEntry.Text);
            }

            
            //GET messages
            var client = new RestClient("https://slack.com/api/conversations.history?token=" + TokenEntry.Text + "&channel=" + ChannelEntry.Text + "&pretty=1");
            var request = new RestRequest(Method.GET);            
            IRestResponse<RootObject> response = client.Execute<RootObject>(request);
            string value = response.Content;
            var fetch = JsonConvert.DeserializeObject<RootObject>(value);

            // Reset deleted messages text
            ResponseText.Text = "Deleted Messages:\n";

            //Delete messages and show user which messages were deleted
            foreach (SlackPurge.Message message in fetch.messages)
            {
                if(message.user == UserEntry.Text)
                {
                    client = new RestClient("https://slack.com/api/chat.delete?token=" + TokenEntry.Text + "&channel=" + ChannelEntry.Text + "&ts=" + message.ts + "&pretty=1");
                    client.Execute(request);

                    ResponseText.Text += message.user + ": " + message.text + "\n";
                    //Thread.Sleep(1200); // 1200 is the minimum for it to work while never hitting the api limit
                }

            }
            Console.WriteLine(fetch.messages[0].text);
            

        }

           
    }
}
