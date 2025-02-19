namespace Cat.Breeds {
    public class Tuxedo : Cat {
        protected override void Awake() {
            base.Awake();
            if (catName == null) catName = "Tuxedo";
            catBreed = "Tuxedo";
        }
    }
}