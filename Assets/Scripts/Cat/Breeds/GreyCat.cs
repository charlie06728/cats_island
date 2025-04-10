using Player;

namespace Cat.Breeds {
    public class GreyCat : Cat {
        protected override void Awake() {
            base.Awake();
            if (catName == null) catName = "Grey Cat";
            catBreed = "Grey Cat";
            favTreat = Items.Cookie;
        }
    }
}