#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("3F9RXm7cX1Rc3F9fXspUNtlc5UEjKuhv2SxG14BvU3Kf+MbIsWla7TboERvDbwlnJqBg1kiY38dcbuQLLEHbP7+m3nAPrbCpbcDprB4osy9f9/0oI8sCH9zWmU8gtl5YNTOdvUZuweHrBrB8sswHkKc+TOpAtlfKsCn2WA1iICgIul7WSXsdGkqWn+UjbJoPMcfLycqv2uehh7eUTijGNm7cX3xuU1hXdNgW2KlTX19fW15dyFoktkAJyezp4ROS/nHVRRs6DI5UU94fCCWE7/GXQHLy4OldIMgiY27gm53ep0RBKf+eXCp78Fi1azFRxxA3vLD3qNqn1VUb6NXfMZ/vlZwp1UtCWSrVcPJed98F3XXKJHgpuGh4eIj0oOKUjVxdX15f");
        private static int[] order = new int[] { 1,5,3,11,8,7,11,12,12,9,12,13,13,13,14 };
        private static int key = 94;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
