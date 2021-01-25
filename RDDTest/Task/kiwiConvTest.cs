using IDAUtil;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using KIWI.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace IDAUnitTest.Task {
    /// <summary>
    /// Testing kiwi conversions and stuff
    /// </summary>
    [TestClass]
    public class kiwiConvTest {

        [TestMethod]
        public void kiwiConversions_Should_RemoveItemsFromZV04IList_When_ExceptionListSoldToAndMaterialMatchZV04IListSoldToAndMaterial() {
            KiwiConversionException kiwiExceptionObj = new KiwiConversionException();
            List<ZV04IProperty> zV04s = getZV04IList();

            zV04s = zV04s.Where(x => !(kiwiExceptionObj.material.Contains(x.material) && kiwiExceptionObj.soldTo.Contains(x.soldTo))).ToList();

            Assert.AreEqual(4, zV04s.Count);
            Assert.IsFalse(zV04s.Contains(new ZV04IProperty() {
                    soldTo = 57071,
                    material = 322413
                }));
        }

        private List<ZV04IProperty> getZV04IList() {
            List<ZV04IProperty> list = new List<ZV04IProperty>() {
                new ZV04IProperty() {
                    soldTo = 1,
                    material = 10
                },
                  new ZV04IProperty() {
                    soldTo = 57071,
                    material = 322413
                },
                    new ZV04IProperty() {
                    soldTo = 3,
                    material = 30
                },
                    new ZV04IProperty() {
                    soldTo = 4,
                    material = 322412
                },
                    new ZV04IProperty() {
                    soldTo = 56925,
                    material = 5
                }
            };
            return list;
        }
    }
}
