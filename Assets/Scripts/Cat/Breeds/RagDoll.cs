using System;

namespace Cat.Breeds {
    public class RagDoll : Cat {
        
        protected override void Awake() {
            base.Awake();
            if (catName == null) catName = "Rag Doll";
            if (catPreferredSnack == null) catPreferredSnack = "Cookie";
            if (catHabitat == null) catHabitat = "Beach, Catnip Garden";
            if (catBreed == null) catBreed = "Rag Doll";
        }
    }
}