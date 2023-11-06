namespace BookStoreAPI.Enum
{
    public static class EnumClass
    {
        public enum BookOption
        {
            GetById = 1,
            GetByCategoryId = 2,
        }
        public enum BookStringOption
        {
            Search = 2
        }
        public enum OrderOption
        {
            ByUserId=1,
            ByOrderId=2,
            
        }
        public enum OrderStatusOption
        {
            Confirm=1,
            Success=2,
            Fail=3,
            Delete=4,
            Restore=5
        }
        public enum CommonStatusOption
        {
            Delete=1,
            Restore=2,
                Confirm=3,
                Undone=4

        }
        public enum RoleOption
        {
            Admin=1,
            Staff=2,
            User=3,
        }
    }
}
