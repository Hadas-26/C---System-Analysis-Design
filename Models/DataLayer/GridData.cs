using Microsoft.AspNetCore.Mvc;

namespace MedicaidEmploymentVerificationApplication.Models.DataLayer
{
    public abstract class GridData
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public string SortDirection { get; set; } = "asc";
        public string SortField { get; set; } = string.Empty;

        //May need a then by field

        

        public int GetTotalPages(int count) => (count + PageSize - 1) / PageSize;

        public void SetSortAndDirection(string newSortField, GridData current)
        {
            SortField = newSortField;

            if (current.SortField.EqualsNoCase(newSortField) && current.SortDirection == "asc")
            {
                SortDirection = "desc";
            }
            else
            {
                SortDirection = "asc";
            }
        }

        public GridData Clone() => (GridData)this.MemberwiseClone();

        public virtual Dictionary<string, string> ToDictionary()
        {
            var routeValues = new Dictionary<string, string>
            {
                { "pageNumber", PageNumber.ToString() },
                { "pageSize", PageSize.ToString() },
                { "sortField", SortField },
                { "sortDirection", SortDirection },
            };
            return routeValues;
        }

       
    }
}
