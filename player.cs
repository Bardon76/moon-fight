using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    float mainSpeed = 50.0f;
    float shiftAdd = 250.0f;
    float maxShift = 1000.0f;
    float camSens = 0.25f; 
    private Vector3 lastMouse = new Vector3(255, 255, 255); 
    private float totalRun = 1.0f;

    float x, y, z;
    float startx, starty, startz;
    float laserx, lasery, laserz;
    float winkel;
    float hitmove;
    float jonesx, jonesy, jonesz;
    float zombiex, zombiey, zombiez;
    float dragonx, dragony, dragonz;
    float capoeirax, capoeiray, capoeiraz;

    bool hit = false;
    bool jonesturn = false;
    bool zombieturn = false;
    bool dragonturn = false;
    bool getroffen = false;
    bool zombieschalter = true;
    bool capoeiraschalter = true;
    bool zweitermoonschalter = true;

    static int level = -1;
    static int lives = 3;
    static int hp = 30;
    int joneshp = 10;
    int jonesr = 1;
    int joneskill = 0;
    int zombiehp = 15;
    int zombier = 1;
    int zombiekill = 0;
    int capoeirahp = 20;
    int capoeirar = 1;
    int capoeirakill = 0;

    public GameObject laser;
    public GameObject jones;
    public GameObject zombie;
    public GameObject capoeira;
    public GameObject anfang;
    public GameObject dragon;
    public GameObject ende;
    public GameObject game_over;

    public Text level_anz;
    public Text lives_anz;
    public Text hp_anz;
    public Text enemyhp_anz;

    // Start is called before the first frame update
    void Start()
    {
        startx = transform.position.x;
        starty = transform.position.y;
        startz = transform.position.z;

        jonesx = jones.transform.position.x;
        jonesy = jones.transform.position.y;
        jonesz = jones.transform.position.z;

        zombiex = zombie.transform.position.x;
        zombiey = zombie.transform.position.y;
        zombiez = zombie.transform.position.z;

        dragonx = dragon.transform.position.x;
        dragony = dragon.transform.position.y;
        dragonz = dragon.transform.position.z;

        capoeirax = capoeira.transform.position.x;
        capoeiray = capoeira.transform.position.y;
        capoeiraz = capoeira.transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if(hp<0)
        {
            lives--;
            hp = 30;
        }

        if(lives<0)
        {
            lives = 3;
            joneshp = 10;
            joneskill = 0;
            jonesr = 1;
            zombiehp = 15;
            zombier = 1;
            zombiekill = 0;
            capoeirahp = 20;
            capoeirakill = 0;
            capoeirar = 1;
            zombieschalter = true;
            capoeiraschalter = true;
            zweitermoonschalter = true;
            level = 99;
            SceneManager.LoadScene(5);
        }

        if(level==99)
        {
            game_over.transform.Rotate(0, 0.075f, 0);

            if(Input.GetKeyDown(KeyCode.Space)) { level = 1;SceneManager.LoadScene(1); }
        }

        if(level==-1) { SceneManager.LoadScene(0); level = 0; }

        if (level == 0)
        {
            anfang.transform.Rotate(0, 0.075f, 0);

            if (Input.GetKeyDown(KeyCode.Space)) { level = 1; SceneManager.LoadScene(1); }

        }

        if(level==10)
        {
            ende.transform.Rotate(0, 0.075f, 0);

            if (Input.GetKeyDown(KeyCode.Space)) { Application.Quit(); }
        }

        if (level > 0)
        {
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;

            if (z < -120) z = -120;
            if (z > 400) z = 400;
            if (x < -19) x = -19;
            if (x > 320) x = 320;



            if (getroffen)
            {
                x -= 5;
                z -= 5;
                getroffen = false;
            }

            transform.position = new Vector3(x, starty + 1, z);

            level_anz.text = "Level: " + level;
            lives_anz.text = "Lives: " + lives;

            hp_anz.text = "Hitpoints: " + hp + " / 30";
            if (level == 1) enemyhp_anz.text = "Jones HP: " + joneshp + " / 10";
            if(level==2)
            {
                float jonesentf, zombieentf;
                jonesentf = Mathf.Sqrt(Mathf.Pow((jonesx - x), 2) + Mathf.Pow((jonesz - z), 2));
                zombieentf = Mathf.Sqrt(Mathf.Pow((zombiex - x), 2) + Mathf.Pow((zombiez - z), 2));
                if(jonesentf<zombieentf || jonesentf==zombieentf) enemyhp_anz.text = "Jones HP: " + joneshp + " / 10";
                else enemyhp_anz.text = "Zombie HP: " + zombiehp + " / 15";

            }
            if (level == 3)
            {
                float capoeiraentf, jonesentf2, zombieentf2;
                jonesentf2 = Mathf.Sqrt(Mathf.Pow((jonesx - x), 2) + Mathf.Pow((jonesz - z), 2));
                zombieentf2 = Mathf.Sqrt(Mathf.Pow((zombiex - x), 2) + Mathf.Pow((zombiez - z), 2));
                capoeiraentf = Mathf.Sqrt(Mathf.Pow((capoeirax - x), 2) + Mathf.Pow((capoeiraz - z), 2));
                if (jonesentf2 < zombieentf2 && jonesentf2 < capoeiraentf) enemyhp_anz.text = "Jones HP: " + joneshp + " / 10";
                if (jonesentf2 > zombieentf2 && zombieentf2 < capoeiraentf) enemyhp_anz.text = "Zombie HP: " + zombiehp + " / 10";
                if (capoeiraentf < zombieentf2 && jonesentf2 > capoeiraentf) enemyhp_anz.text = "Faceless HP: " + capoeirahp + " / 10";

            }

            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
            //Mouse  camera angle done.  

            //Keyboard commands
            float f = 0.0f;
            Vector3 p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            if (Input.GetKey(KeyCode.Space))
            { //If player wants to move on X and Z axis only
                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else
            {
                transform.Translate(p);
            }

            if (Input.GetMouseButtonDown(0))
            {
                hit = true;
                CancelInvoke("nohit");
                hitmove = 0.1f;
                winkel = transform.eulerAngles.y;
                laserx = transform.position.x;
                lasery = transform.position.y - 1;
                laserz = transform.position.z;
                Invoke("nohit", 2);
            }

            if (hit)
            {
                hitmove += 0.4f;
                laser.transform.position = new Vector3(laserx, lasery, laserz);
                if (winkel < 22.5f) { laserz += hitmove; }
                if(winkel>22.4f && winkel<45) { laserz += hitmove; laserx += hitmove / 2; }
                if (winkel > 44 && winkel < 67.5f) { laserz += hitmove; laserx += hitmove; }
                if (winkel > 67.4f && winkel < 90) { laserz += hitmove / 2; laserx += hitmove; }
                if (winkel > 89 && winkel < 112.5f) { laserx += hitmove; }
                if(winkel>112.4f && winkel<135) { laserx += hitmove; laserz -= hitmove / 2; }
                if (winkel > 134 && winkel < 157.5f) { laserx += hitmove; laserz -= hitmove; }
                if(winkel>157.4f && winkel<180) { laserx += hitmove / 2; laserz -= hitmove; }
                if (winkel > 179 && winkel < 202.5f) { laserz -= hitmove; }
                if(winkel>202.4f && winkel<225) { laserz -= hitmove; laserx -= hitmove / 2; }
                if (winkel > 224 && winkel < 247.5f) { laserx -= hitmove; laserz -= hitmove; }
                if(winkel>247.4f && winkel<270) { laserx -= hitmove; laserz -= hitmove / 2; }
                if (winkel > 269 && winkel < 292.5f) { laserx -= hitmove; }
                if(winkel>292.4f && winkel<315) { laserx -= hitmove;laserz += hitmove / 2; }
                if (winkel > 314 && winkel<337.5f) { laserx -= hitmove; laserz += hitmove; }
                if(winkel>337.4f) { laserx -= hitmove / 2; laserz += hitmove; }
            }

            if (!dragonturn) dragonz -= 1f;
            else dragonz += 1f;
            if (dragonz < -120) { dragonturn = true; dragon.transform.Rotate(0, 180, 0); }
            if (dragonz > 400) { dragonturn = false; dragon.transform.Rotate(0, 180, 0); }
            dragon.transform.position = new Vector3(dragonx, dragony, dragonz);

            if (level > 0)
            {
                float joneswayx, joneswayz;
                joneswayx = jonesx - x;
                if (joneswayx > 0) jonesx -= 0.1f;
                if (joneswayx < -1) jonesx += 0.1f;
                joneswayz = jonesz - z;
                if (joneswayz > 0) { jonesz -= 0.1f; if (jonesr != 1) jones.transform.Rotate(0, 180, 0); jonesr = 1; }
                if (joneswayz < -1) { jonesz += 0.1f; if (jonesr != 3) jones.transform.Rotate(0, 180, 0); jonesr = 3; }

                jones.transform.position = new Vector3(jonesx, jonesy, jonesz);

                float jonesxdiff, joneszdiff, jonesydiff;
                jonesxdiff = Mathf.Abs(jonesx - x);
                jonesydiff = Mathf.Abs(jonesy - y);
                joneszdiff = Mathf.Abs(jonesz - z);
                if (jonesxdiff < 2 && jonesydiff < 2 && joneszdiff < 2)
                {
                    hp--;

                    getroffen = true;
                }

                float laserxdiff, laserydiff, laserzdiff;
                laserxdiff = Mathf.Abs(laserx - jonesx);
                laserydiff = Mathf.Abs(lasery - jonesy);
                laserzdiff = Mathf.Abs(laserz - jonesz);
                if (laserxdiff < 2 && laserydiff < 2 && laserzdiff < 2) joneshp--;
                if (joneshp < 0) { jones.transform.position = new Vector3(0, -20, 0); jonesy = -20; Invoke("new_jones", 0.1f); joneshp = 10; }

                if (joneskill == 3 && zombieschalter) { level++; zombieschalter = false; zombiekill = -1; Invoke("new_zombie", 0.1f); }

                if (level ==2 || level==3 || level == 5 || level == 6 || level == 8 || level == 9)
                {
                    float zombiewayx, zombiewayz;
                    zombiewayx = zombiex - x;
                    if (zombiewayx > 0) zombiex -= 0.1f;
                    if (zombiewayx < -1) zombiex += 0.1f;
                    zombiewayz = zombiez - z;
                    if (zombiewayz > 0) { zombiez -= 0.1f; if (zombier != 1) zombie.transform.Rotate(0, 180, 0); zombier = 1; }
                    if (zombiewayz < -1) { zombiez += 0.1f; if (zombier != 3) zombie.transform.Rotate(0, 180, 0); zombier = 3; }

                    zombie.transform.position = new Vector3(zombiex, zombiey, zombiez);

                    float zombiexdiff, zombiezdiff, zombieydiff;
                    zombiexdiff = Mathf.Abs(zombiex - x);
                    zombieydiff = Mathf.Abs(zombiey - y);
                    zombiezdiff = Mathf.Abs(zombiez - z);
                    if (zombiexdiff < 2 && zombieydiff < 2 && zombiezdiff < 2)
                    {
                        hp--;

                        getroffen = true;
                    }

                    //float laserxdiff, laserydiff, laserzdiff;
                    laserxdiff = Mathf.Abs(laserx - zombiex);
                    laserydiff = Mathf.Abs(lasery - zombiey);
                    laserzdiff = Mathf.Abs(laserz - zombiez);
                    if (laserxdiff < 2 && laserydiff < 2 && laserzdiff < 2) zombiehp--;
                    if (zombiehp < 0) { zombie.transform.position = new Vector3(0, -20, 0); zombiey = -20; Invoke("new_zombie", 0.1f); zombiehp = 15; }
                    if (zombiekill == 3 && capoeiraschalter) { level++; capoeiraschalter = false; capoeirakill = -1; Invoke("new_capoeira", 0.1f); }
                }

                if (level == 3 || level == 6 || level == 9)
                {
                    float capoeirawayx, capoeirawayz;
                    capoeirawayx = capoeirax - x;
                    if (capoeirawayx > 0) capoeirax -= 0.1f;
                    if (capoeirawayx < -1) capoeirax += 0.1f;
                    capoeirawayz = capoeiraz - z;
                    if (capoeirawayz > 0) { capoeiraz -= 0.1f; if (capoeirar != 1) capoeira.transform.Rotate(0, 180, 0); capoeirar = 1; }
                    if (capoeirawayz < -1) { capoeiraz += 0.1f; if (capoeirar != 3) capoeira.transform.Rotate(0, 180, 0); capoeirar = 3; }

                    capoeira.transform.position = new Vector3(capoeirax, capoeiray, capoeiraz);

                    float capoeiraxdiff, capoeirazdiff, capoeiraydiff;
                    capoeiraxdiff = Mathf.Abs(capoeirax - x);
                    capoeiraydiff = Mathf.Abs(capoeiray - y);
                    capoeirazdiff = Mathf.Abs(capoeiraz - z);
                    if (capoeiraxdiff < 2 && capoeiraydiff < 2 && capoeirazdiff < 2)
                    {
                        hp--;

                        getroffen = true;
                    }

                    //float laserxdiff, laserydiff, laserzdiff;
                    laserxdiff = Mathf.Abs(laserx - capoeirax);
                    laserydiff = Mathf.Abs(lasery - capoeiray);
                    laserzdiff = Mathf.Abs(laserz - capoeiraz);
                    if (laserxdiff < 2 && laserydiff < 2 && laserzdiff < 2) capoeirahp--;
                    if (capoeirahp < 0) { capoeira.transform.position = new Vector3(0, -20, 0); capoeiray = -20; Invoke("new_capoeira", 0.1f); capoeirahp = 20; }
                    if (capoeirakill == 3 && zweitermoonschalter) 
                    { 
                        level = 4; 
                        zweitermoonschalter = false; 
                        SceneManager.LoadScene(2);
                        joneskill = 0;
                        zombiekill = 0;
                        capoeirakill = 0;
                        joneshp = 10;
                        zombiehp = 15;
                        capoeirahp = 20;
                        jonesr = 1;
                        zombier = 1;
                        capoeirar = 1;
                        zombieschalter = true;
                        capoeiraschalter = true;
                    }
                    if (capoeirakill == 3 && level == 6)
                    {
                        level = 7;
                        zweitermoonschalter = false;
                        SceneManager.LoadScene(3);
                        joneskill = 0;
                        zombiekill = 0;
                        capoeirakill = 0;
                        joneshp = 10;
                        zombiehp = 15;
                        capoeirahp = 20;
                        jonesr = 1;
                        zombier = 1;
                        capoeirar = 1;
                        zombieschalter = true;
                        capoeiraschalter = true;
                    }
                    if (capoeirakill == 3 && level == 9)
                    {
                        level = 10;
                        SceneManager.LoadScene(4);
                    }
                }

            }

                       
        }

    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    void nohit()
    {
        hit = false;
        hitmove = 0.1f;
    }

    void new_jones()
    {
        jonesx = Random.Range(-19, 320);
        jonesz = Random.Range(-120, 400);
        jonesy = 0.05f;
        joneshp = 10;
        joneskill++;
    }

    void new_zombie()
    {
        zombiex = Random.Range(-19, 320);
        zombiez = Random.Range(-120, 400);
        zombiey = 0.05f;
        zombiehp = 15;
        zombiekill++;
    }

    void new_capoeira()
    {
        capoeirax = Random.Range(-19, 320);
        capoeiraz = Random.Range(-120, 400);
        capoeiray = 0.05f;
        capoeirahp = 20;
        capoeirakill++;
    }
}
