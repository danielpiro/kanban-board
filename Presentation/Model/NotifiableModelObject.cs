namespace Presentation.Model
{
    public class NotifiableModelObject : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        protected NotifiableModelObject(BackendController controller)
        {
            Controller = controller;
        }
    }
}
