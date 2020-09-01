namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User
    {
        public readonly string Email;
        public readonly string Nickname;
        internal User(string email, string nickname)
        {
            Email = email;
            Nickname = nickname;
        }
    }
}
