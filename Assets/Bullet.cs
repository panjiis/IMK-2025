using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 50f;
    private Vector3 startPosition;

    // Target1
    public static int shotCount = 0;
    public static FollowPlayer currentFollower = null;
    public static HashSet<GameObject> hitTargets = new HashSet<GameObject>();

    // Target2
    public static int shotCount2 = 0;
    public static FollowPlayer currentFollower2 = null;
    public static HashSet<GameObject> hitTargets2 = new HashSet<GameObject>();

    // Target3
    public static int shotCount3 = 0;
    public static GameObject lastTarget3 = null;
    public static HashSet<GameObject> hitTargets3 = new HashSet<GameObject>();

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled > maxDistance)
        {
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Transform player = GameObject.FindWithTag("Player")?.transform;
        Debug.Log("Bullet menabrak: " + other.gameObject.name);

        // Handle "sleep"
        if (other.CompareTag("sleep"))
        {
            shotCount = 0;
            currentFollower = null;
            shotCount2 = 0;
            currentFollower2 = null;
            shotCount3 = 0;
            lastTarget3 = null;

            int totalPenguranganTarget1 = 0;
            int totalPenguranganTarget2 = 0;

            // Reset dan kurangi umur semua target di scene
            string[] tags = { "Target", "Target2", "Target3" };
            foreach (string tag in tags)
            {
                GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject obj in targets)
                {
                    if (obj != null)
                    {
                        obj.transform.localScale = Vector3.one;

                        ObjectLifetime ol = obj.GetComponent<ObjectLifetime>();
                        if (ol != null)
                        {
                            ol.DecreaseAge();
                            if (ol.age <= 0)
                            {
                                Destroy(obj);
                                if (tag == "Target") totalPenguranganTarget1++;
                                if (tag == "Target2") totalPenguranganTarget2++;
                            }
                        }
                    }
                }
            }

            // Kosongkan hitSet agar objek bisa ditembak ulang
            hitTargets.Clear();
            hitTargets2.Clear();
            hitTargets3.Clear();

            // === HAPUS Target2 dengan umur terkecil ===
            int target1Count = GameObject.FindGameObjectsWithTag("Target").Length;
            int jumlahHapusT2 = target1Count / 2;
            totalPenguranganTarget2 += jumlahHapusT2;

            GameObject[] allTarget2 = GameObject.FindGameObjectsWithTag("Target2");
            List<(GameObject obj, int age)> target2List = new List<(GameObject, int)>();
            foreach (GameObject obj in allTarget2)
            {
                ObjectLifetime ol = obj.GetComponent<ObjectLifetime>();
                if (ol != null)
                {
                    target2List.Add((obj, ol.age));
                }
            }
            target2List.Sort((a, b) => a.age.CompareTo(b.age));
            for (int i = 0; i < Mathf.Min(jumlahHapusT2, target2List.Count); i++)
            {
                GameObject toDestroy = target2List[i].obj;
                Destroy(toDestroy);
                Debug.Log($"Menghapus Target2 (berdasar Target1): {toDestroy.name} age={target2List[i].age}");
            }

            // === HAPUS Target3 dengan umur terkecil ===
            int jumlahHapusT3 = target2List.Count / 2;

            GameObject[] allTarget3 = GameObject.FindGameObjectsWithTag("Target3");
            List<(GameObject obj, int age)> target3List = new List<(GameObject, int)>();
            foreach (GameObject obj in allTarget3)
            {
                ObjectLifetime ol = obj.GetComponent<ObjectLifetime>();
                if (ol != null)
                {
                    target3List.Add((obj, ol.age));
                }
            }
            target3List.Sort((a, b) => a.age.CompareTo(b.age));
            for (int i = 0; i < Mathf.Min(jumlahHapusT3, target3List.Count); i++)
            {
                GameObject toDestroy = target3List[i].obj;
                Destroy(toDestroy);
                Debug.Log($"Menghapus Target3 (berdasar Target2): {toDestroy.name} age={target3List[i].age}");
            }

            // === HITUNG & BANDINGKAN ===
            int totalHilang = (totalPenguranganTarget1 + totalPenguranganTarget2) * 5;
            int target3Tersisa = GameObject.FindGameObjectsWithTag("Target3").Length;
            int selisih = target3Tersisa - totalHilang;

            if (selisih > 0)
            {
                // Ambil ulang list target3, urutkan, dan hapus selisihnya
                GameObject[] sisaTarget3 = GameObject.FindGameObjectsWithTag("Target3");
                List<(GameObject obj, int age)> sisaList = new List<(GameObject, int)>();
                foreach (GameObject obj in sisaTarget3)
                {
                    ObjectLifetime ol = obj.GetComponent<ObjectLifetime>();
                    if (ol != null)
                    {
                        sisaList.Add((obj, ol.age));
                    }
                }

                sisaList.Sort((a, b) => a.age.CompareTo(b.age));
                for (int i = 0; i < Mathf.Min(selisih, sisaList.Count); i++)
                {
                    GameObject toDestroy = sisaList[i].obj;
                    Destroy(toDestroy);
                    Debug.Log($"Menghapus Target3 (karena kelebihan dari total hilang*5): {toDestroy.name} age={sisaList[i].age}");
                }
            }

            Debug.Log("Sleep selesai: Target1-" + totalPenguranganTarget1 + ", Target2-" + totalPenguranganTarget2 + ", T3 delete tambahan (jika perlu): " + selisih);
            return;
        }




        if (HandleTargetHit(other, "Target", ref shotCount, ref currentFollower, hitTargets, player)) return;
        if (HandleTargetHit(other, "Target2", ref shotCount2, ref currentFollower2, hitTargets2, player)) return;

        if (other.CompareTag("Target3") && !hitTargets3.Contains(other.gameObject))
        {
            hitTargets3.Add(other.gameObject);
            shotCount3++;
            lastTarget3 = other.gameObject;
            Debug.Log("Target3 hit, count: " + shotCount3);
            return;
        }

        if (other.CompareTag("plane") && shotCount3 > 0 && lastTarget3 != null)
        {
            GameObject clone = Instantiate(lastTarget3, lastTarget3.transform.position + Vector3.right * 1.5f, Quaternion.identity);
            clone.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            clone.tag = "Target3";

            FollowPlayer fp = clone.GetComponent<FollowPlayer>();
            if (fp != null)
                fp.hasBeenShot = true;

            ObjectLifetime ol = clone.GetComponent<ObjectLifetime>();
            if (ol == null) ol = clone.AddComponent<ObjectLifetime>();
            ol.age = 4;

            hitTargets3.Add(clone);
            shotCount3--;
            Debug.Log("Plane hit: clone Target3 created, count: " + shotCount3);
            return;
        }
    }

    bool HandleTargetHit(Collider other, string tag, ref int shotCount, ref FollowPlayer currentFollower, HashSet<GameObject> hitSet, Transform player)
    {
        if (other.CompareTag(tag) && !hitSet.Contains(other.gameObject))
        {
            hitSet.Add(other.gameObject);
            shotCount++;

            FollowPlayer newFollower = other.GetComponent<FollowPlayer>();
            if (newFollower != null)
            {
                if (shotCount % 2 == 1)
                {
                    if (player != null)
                    {
                        newFollower.ActivateFollow(player);
                        currentFollower = newFollower;
                    }
                }
                else
                {
                    if (currentFollower != null)
                    {
                        currentFollower.DeactivateFollow();
                        currentFollower = null;
                    }

                    GameObject clone = Instantiate(other.gameObject, other.transform.position + Vector3.right * 1.5f, Quaternion.identity);
                    clone.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    clone.tag = tag;

                    FollowPlayer cloneFP = clone.GetComponent<FollowPlayer>();
                    if (cloneFP != null)
                        cloneFP.hasBeenShot = true;

                    ObjectLifetime ol = clone.GetComponent<ObjectLifetime>();
                    if (ol == null) ol = clone.AddComponent<ObjectLifetime>();
                    ol.age = 4;

                    hitSet.Add(clone);
                }
            }

            return true;
        }
        return false;
    }

    void ResetTargets(HashSet<GameObject> targets, ref int count, ref FollowPlayer current)
    {
        count = 0;
        current = null;
        foreach (GameObject obj in targets)
        {
            if (obj != null)
            {
                obj.transform.localScale = Vector3.one;

                ObjectLifetime ol = obj.GetComponent<ObjectLifetime>();
                if (ol != null)
                    ol.DecreaseAge();
            }
        }

        // Bersihkan semua agar bisa ditembak ulang
        targets.Clear();
    }


    void ResetTargets3()
    {
        shotCount3 = 0;
        lastTarget3 = null;
        foreach (GameObject obj in hitTargets3)
        {
            if (obj != null)
            {
                obj.transform.localScale = Vector3.one;

                ObjectLifetime ol = obj.GetComponent<ObjectLifetime>();
                if (ol != null)
                    ol.DecreaseAge();
            }
        }

        // Bersihkan agar bisa ditembak ulang
        hitTargets3.Clear();
    }

}
