using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Soccer.ViewModels;

namespace Soccer.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchPage : ContentPage
	{
		public MatchPage ()
		{
			InitializeComponent ();
            var vm = MatchViewModel.GetInstance();
            base.Appearing += (object sender, EventArgs e) =>
            {
                vm.RefreshCommand.Execute(this);
            };
        }
	}
}