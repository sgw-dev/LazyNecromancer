using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour {
    
    [Range(1f,1.3f)]
    [SerializeField]
    float animBufferAdjustment;

    public Transform fallFrom;
    public GameObject SlimeBoss;
    public GameObject FakeBoss; 
    public Color startColor;
    public AnimationCurve colorFade;
    public float shadowAliveTime;
    Animator spawnerAnim;
    SpriteRenderer sr;

    GameObject slimeBoss;
    readonly string SPAWN = "Spawn";
    bool _fadelock = false;
    float timer;
    Animator fakebossanim;
    GameObject fakebossgo;
    public static readonly Vector3 offscreen = new Vector3(-5000,-5000,0);

    void Start() {
        
        spawnerAnim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = startColor;

        fakebossgo = GameObject.Instantiate(FakeBoss,null);
        fakebossgo.transform.position = transform.position;
        fakebossanim = fakebossgo.GetComponent<Animator>();

        slimeBoss = GameObject.Instantiate(SlimeBoss,null);
        slimeBoss.SetActive(false);
        slimeBoss.transform.position = offscreen;
        AnimationClip[] clips = spawnerAnim.runtimeAnimatorController.animationClips;
        
        AnimationClip shadow = clips.ToList<AnimationClip>().Single<AnimationClip>(c => c.name.Equals("ShadowAnimation"));
        shadowAliveTime=shadow.averageDuration/.5f;//not sure how to read the emultiplier value
        shadowAliveTime *= animBufferAdjustment;

    }

    void Update() {
        // if(Input.GetKeyDown(KeyCode.Alpha1)) {
        //     TriggerBossStart();
        // }
    }

    //use this to start the boss
    public void TriggerBossStart() {
        sr.enabled=true;
        StartBoss();
    }

    async void StartBoss() {

        if( !_fadelock ) {
            _fadelock = true;
            spawnerAnim.SetTrigger(SPAWN);

            timer = 0f;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            float total = colorFade.keys[colorFade.keys.Length - 1].time;//this is 1 by default
            /*
                Color offset
                original
                |
                | Color =  original - original * evalauated 
                V

                black (0,0,0)

            */
            Color alpha1 = new Color(0f,0f,0f,1f);

            Vector3 startfalling = fallFrom.position;
            Vector3 endFall      = transform.position; 
            
            slimeBoss.transform.position = startfalling;
            slimeBoss.GetComponent<SlimeBehaviour>().enabled=false;
            slimeBoss.GetComponent<Collider2D>().enabled=false;
            slimeBoss.GetComponentsInChildren<Collider2D>().ToList<Collider2D>().ForEach(c => c.enabled=false);
            slimeBoss.GetComponentsInChildren<SlimeAttackRange>().ToList<SlimeAttackRange>().ForEach(c => c.enabled=false);
            slimeBoss.SetActive(true);

            while(timer < shadowAliveTime) {
            
                float val = colorFade.Evaluate(timer);
                sr.color = startColor -  startColor * val + alpha1;   
                if(val>.9) {//use ruler to find a good value, in the child object
                    sr.enabled = false;
                    PlayBossCrushedAnimation();
                }             
                slimeBoss.transform.position = Vector3.Lerp(startfalling, endFall ,val);


                timer+=Time.deltaTime;
                await Task.Yield();
            }

            //force exact position, and enable boss comps
            slimeBoss.transform.position = transform.position;
            slimeBoss.GetComponent<Collider2D>().enabled=false;
            slimeBoss.GetComponentsInChildren<Collider2D>().ToList<Collider2D>().ForEach(c => c.enabled=true);
            slimeBoss.GetComponentsInChildren<SlimeAttackRange>().ToList<SlimeAttackRange>().ForEach(c => c.enabled=true);
            slimeBoss.GetComponent<SlimeBehaviour>().enabled=true;
            _fadelock = false;
        }
    }

    public void PlayBossCrushedAnimation() {
        fakebossgo.SetActive(false);
        if(fakebossanim!=null) {
            fakebossanim.SetTrigger("crushed");
        }
    }

    public void EndBossSpawnAnimation() {
        this.gameObject.SetActive(false);//deactivate the object
    }
}