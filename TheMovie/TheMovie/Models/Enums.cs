namespace TheMovie.Models
{
    public class Enums
    {
        public enum MovieCategory
        {            
            TopRated,
            Discover
        }

        public static string PathCategoryMovie(MovieCategory category)
        {
            switch (category)
            {                
                case MovieCategory.TopRated:
                    return "/movie/top_rated";

                case MovieCategory.Discover:
                    return "/discover/movie";
                default:
                    return "";
            }
        }

        public static string NameCategoryMovie(MovieCategory category)
        {
            switch (category)
            {
                

                case MovieCategory.TopRated:
                    return "Top Rated";

                case MovieCategory.Discover:
                    return "Discover";
                default:
                    return "";
            }
        }
    }
}
