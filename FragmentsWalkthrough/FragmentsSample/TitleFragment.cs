using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace FragmentSample
{
    public class TitleFragment : ListFragment
    {
        int selectedIndex;
        bool showingTwoFragments;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            ListAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItemActivated1, Shakespeare.Titles);

            var quoteContainer = Activity.FindViewById(Resource.Id.playquote_container);
            showingTwoFragments = quoteContainer != null &&
                                  quoteContainer.Visibility == ViewStates.Visible;

            if (savedInstanceState != null)
            {
                selectedIndex = savedInstanceState.GetInt("current_play_id", 0);
            }

            if (showingTwoFragments)
            {
                ListView.ChoiceMode = ChoiceMode.Single;
                ShowDetails(selectedIndex);
            }
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("current_play_id", selectedIndex);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ShowDetails(position);
        }

        void ShowDetails(int playId)
        {
            if (showingTwoFragments)
            {
                ListView.SetItemChecked(selectedIndex, true);

                var playQuoteFragment = (PlayQuoteFragment) FragmentManager.FindFragmentById(Resource.Id.playquote_container);

                if (playQuoteFragment == null || playQuoteFragment.PlayId != playId)
                {
                    var quoteFrag = PlayQuoteFragment.NewInstance(selectedIndex);
                    var ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.playquote_container, quoteFrag);

                    ft.SetTransition(FragmentTransit.FragmentFade);
                    ft.Commit();
                }
            }
            else
            {
                var intent = new Intent(Activity, typeof(PlayQuoteActivity));
                intent.PutExtra("current_play_id", playId);
                StartActivity(intent);
            }
        }
    }
}
