namespace Cat.Breeds {
    public class GreyTabby : Cat {
        protected override void Awake() {
            base.Awake();
            if (catName == null) catName = "Grey Tabby";
            catBreed = "Grey Tabby";
        }
    }
}