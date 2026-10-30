/*
 * Written by Josh Green
 */

namespace MedicaidEmploymentVerificationApplication.Utilities
{
    public static class StaticData
    {
        public static Dictionary<string, string> States = new Dictionary<string, string> 
        { 
            { "Alabama", "AL" }, 
            { "Alaska", "AK" }, 
            { "Arizona", "AZ" }, 
            { "Arkansas", "AR" }, 
            { "California", "CA" }, 
            { "Colorado", "CO" }, 
            { "Connectiut", "CT" }, 
            { "Delaware", "DE" }, 
            { "Florida", "FL" }, 
            { "Georgia", "GA" }, 
            { "Hawaii", "HI" }, 
            { "Idaho", "ID" }, 
            { "Illinois", "IL" }, 
            { "Indiana", "IN" }, 
            { "Iowa", "IA" }, 
            { "Kansas", "KS" }, 
            { "Kentucky", "KY" }, 
            { "Louisiana", "LA" }, 
            { "Maine", "ME" }, 
            { "Maryland", "MD" }, 
            { "Massachusetts", "MA" }, 
            { "Michigan", "MI" }, 
            { "Minnesota", "MN" }, 
            { "Missisippi", "MS" }, 
            { "Missouri", "MO" }, 
            { "Montana", "MT" }, 
            { "Nebraska", "NE" }, 
            { "Nevada", "NV" }, 
            { "New Hampshire", "NH" }, 
            { "New Jersey", "NJ" }, 
            { "New Mexico", "NM" }, 
            { "New York", "NY" }, 
            { "North Carolina", "NC" }, 
            { "North Dakota", "ND" }, 
            { "Ohio", "OH" }, 
            { "Oklahoma", "OK" }, 
            { "Oregon", "OR" }, 
            { "Pennsylvania", "PA" }, 
            { "Rhode Island", "RI" }, 
            { "South Carolina", "SC" }, 
            { "South Dakota", "SD" }, 
            { "Tennessee", "TN" }, 
            { "Texas", "TX" }, 
            { "Utah", "UT" }, 
            { "Vermont", "VT" }, 
            { "Virginia", "VA" }, 
            { "Washington", "WA" }, 
            { "West Virginia", "WV" }, 
            { "Wisconsin", "WI" }, 
            { "Wyoming", "WY" }, 
        };

        public static Dictionary<string, string> FAQs { get; set; } = new Dictionary<string, string>
        {
            { "How do I apply?","If you do not yet have an account, then you must first create one by clicking on log in, then register. " +
                "Otherwise, click the Apply Now button on the left-hand menu and fill out the application. After a " +
                "Medicaid employee has reviewed your application, you will be notified as to whether or not you have met the qualification. " +
                "Be prepared to show documented proof for things like work hours, volunteer hours, disability, and any other claim that requires validation." +
                "You can always look at the status of your application using theApplication Status menu option located on the left-hand menu." 
            },

            {"Are my login credentials protected?","MEVA encrypts user credentials using SHA256 hashing and salting to ensure the highest level  of protection." },

            {"How long does the application reviewing process take?","Typically no longer than a few weeks, but it may take longer if MEVA experiences higher levels of application submissions than usual." }
        };

        public static List<string> Sexes = new List<string> { "Male", "Female" };
    }
}
