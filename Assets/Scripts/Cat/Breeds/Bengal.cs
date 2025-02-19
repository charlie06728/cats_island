namespace Cat.Breeds {
    public class Bengal : Cat {
        protected override void Awake() {
            base.Awake();
            if (catName == null) catName = "Bengal";
            catBreed = "Bengal";
        }
    }
}