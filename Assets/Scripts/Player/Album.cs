using System;

namespace Player {
    public class Album : Item {
        public override void TakeOut() {
            base.TakeOut();
            throw new NotImplementedException();
        }
        
        public override void PutBack() {
            base.PutBack();
            throw new NotImplementedException();
        }
    }
}