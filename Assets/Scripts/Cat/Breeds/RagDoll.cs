using System;

namespace Cat.Breeds {
    public class RagDoll : Cat {
        
        protected override void Awake() {
            base.Awake();
            if (catName == null) catName = "Rag Doll";
            catBreed = "Rag Doll";
        }
    }
}