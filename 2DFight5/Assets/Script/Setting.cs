using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
public class SettingFunction {
    public enum key { left, right, up, down, fire, select, cancel, ability1, ability2, ability3, ability4 };
    public enum BuildTarget {PCStandalone,Mobile,Console};
    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/setting.dat");

        Setting data = Setting.current;
        bf.Serialize(file, data);
    }
}
public class SettingList
{

}
[System.Serializable]
public class Setting {
    public static Setting current = new Setting(SettingFunction.BuildTarget.PCStandalone);
    public KeyCode[] key = new KeyCode[11];
    public SettingFunction.BuildTarget buildTarget;
    public Setting(SettingFunction.BuildTarget buildtarget) {
        buildTarget = buildtarget;
        if (buildtarget == SettingFunction.BuildTarget.PCStandalone) {
            key[(int)SettingFunction.key.up] = KeyCode.W;
            key[(int)SettingFunction.key.down] = KeyCode.S;
            key[(int)SettingFunction.key.left] = KeyCode.A;
            key[(int)SettingFunction.key.right] = KeyCode.D;
            key[(int)SettingFunction.key.select] = KeyCode.Return;
            key[(int)SettingFunction.key.cancel] = KeyCode.Escape;
            key[(int)SettingFunction.key.ability1] = KeyCode.Alpha1;
            key[(int)SettingFunction.key.ability2] = KeyCode.Alpha2;
            key[(int)SettingFunction.key.ability3] = KeyCode.Alpha3;
            key[(int)SettingFunction.key.ability4] = KeyCode.Alpha4;
            key[(int)SettingFunction.key.fire] = KeyCode.Mouse0;
        }
    }
}