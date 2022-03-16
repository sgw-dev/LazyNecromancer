namespace fancy.component.model
{
    public interface IComponent
    {
        public void StartEvent(IProp prop);
        public void Evalulate(float t);
        public void EndEvent();
    }
}