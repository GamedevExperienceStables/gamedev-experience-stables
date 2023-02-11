using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using KinematicCharacterController;

[RequireComponent(typeof(KinematicCharacterMotor))]
public class AudioController : MonoBehaviour
{
    private KinematicCharacterMotor _motor;
    //These variables will all be set in the Inspector tab of Unity's Editor by either us, or the 'FMODStudioFootstepsEditor' script.
    [Header("FMOD Settings")]
    [SerializeField] private FMODUnity.EventReference FootstepsEventPath;    // Use this in the Editor to select our Footsteps Event.
    [SerializeField] private FMODUnity.EventReference JumpingEventPath;      // Use this in the Editor to select our Jumping Event.
    [SerializeField] private string MaterialParameterName;                      // Use this in the Editor to write the name of the parameter that contorls which material the player is currently walking on.
    [SerializeField] private string SpeedParameterName;                         // Use this in the Editor to write the name of the parameter that contorls which footstep speed needs to be heard.
    [SerializeField] private string JumpOrLandParameterName;                    // Use this in the Editor to write the name of the parameter that contorls whether or not a jumping or a landing sound needs to be heard.
    [Header("Playback Settings")]
    [SerializeField] private float StepDistance = 2.0f;                         // Select how far the player must travel before they hear a footstep. This will then remain a constant and will not change.
    [SerializeField] private float RayDistance = 1.2f;                          // Select how far the raycast will travel down to when checking for a floor. This will then remain a constant and will not change.
    [SerializeField] private float StartRunningTime = 0.3f;                     // Set a tmie. If the time between each step the player takes is less than this value, the player will start to hear running footsteps. This will then remain a constant and will not change.
    [SerializeField] private string JumpInputName;                              // In Unity, go Edit -> Project Settings -> Input Manager. Then find the name of the input that controls which key/button the player must press in order to jump (it's proably called "Jump"). Then once you know it's name, write it into this variable in the Inspector tab. This will then remain a constant and will not change.
    public string[] MaterialTypes;                                              // This is an array of strings. In the inspector we can decide how many Material types we have in FMOD by setting the size of this array. Depending on the size, the array will then create a certain amount of strings for us to fill in with the name of each of our footstep materials for our scripts to use. This will then remain a constant and will not change.
    [HideInInspector] public int DefulatMaterialValue;                          // This will be told by the 'FMODStudioFootstepsEditor' script which Material has been set as the defualt. It will then store the value of that Material for outhis script to use. This cannot be changed in the Editor, but a drop down menu created by the 'FMODStudioFootstepsEditor' script can.
    //These variables are used to control when the player executes a footstep.
    private float StepRandom;                                                   // This will be set as random number, which will later be added to the StepDistance to add a little variaiton to the length in steps.
    private Vector3 PrevPos;                                                    // This will old the co-ordinates of the previous postion the player was in during the last frame.
    private float DistanceTravelled;                                            // This will hold a value that how represnt how far the player has travlled since they last took a step.
    //These variables are used when checking the Material type the player is on top of.
    private RaycastHit hit;                                                     // Will holds information about the GameObject that the raycast hits.
    private int F_MaterialValue;                                                // We'll use this to set the value of our FMOD Material Parameter.
    //These booleans will hold values that tell us if the player is touching the ground currently and if they were touching it during the last frame.
    private bool PlayerTouchingGround;                                          // This will hold a value to represent whether or not the player is touching the ground. True = Grounded, False = Not Grounded.
    private bool PreviosulyTouchingGround;                                      // This will hold a value to represent whether or not the player was touching the ground during the last frame. True = Was Grounded, False = Wasn't Grounded.
    //These floats help us determine when the player should be running.
    private float TimeTakenSinceStep;                                           // We'll use this as a timer, to track the time it takes between each step.
    private int F_PlayerRunning;                                                // We'll use to set the value of our FMOD Switch Speed Parameter.

   
   
    
    private void Awake()
    {
        _motor = GetComponent<KinematicCharacterMotor>();
       
    }
    void Start() // This method, and everyting inside of it, is perfomred on the very first frame that is run when we start the game.
    {
        StepRandom = Random.Range(0f, 0.5f);        // 'Step Random' now holds a value. It could be anywhere between 0 and 0.5.
        PrevPos = transform.position;               // 'PrevPos' now holds the location that the player is starting at as co-ordinates (x, y, z).
    }
    void Update() // This method, and everyting insid of it, is perfomred once every frame. If the game is running at an average of 60 frames per second, this method will be perfomred 60 times a second.
    {

        
        //This section checks to see if the player is touching the ground or not, so that a footstep is played whilst the player is in the air. It also determines when the JumpOrLand method should be told to trigger, thus playing the Jump & Land event in FMOD.
        GroundedCheck();                                                   // First, the 'GoundedCheck' method is performed to see if the player is currently standing on the ground or not. This will set the 'PlayerTouchingGround' boolean to either true or false respectively.
        if (PlayerTouchingGround && Input.GetButtonDown(JumpInputName))    // If the player is touching the ground AND the player presses the button to jump at the same time, we knoe that the player character is about to jump, therefore we perform our method to play a sound.
        {
            MaterialCheck();                                               // Before we play a jumping sound, we need to know what material the player has jumped off of. This
            PlayJumpOrLand(true);                                          // The 'PlayJumpOrLand' method is perfomred, triggering our 'Jump & Land' event in FMOD to play. We also pass through it's parameter brackets the 'true' boolean value for our method to store inside a vaiable and use to play a jump sound with.
        }
        if (!PreviosulyTouchingGround && PlayerTouchingGround)             // If the player wasn't touching the ground during the last frame, but is touching the ground during the current frame, then that means they must have just landed on the ground, therefore we perform out methods and play a sound.
        {
            MaterialCheck();                                               // Before we play a landing sound, we need to know what material the player has landed on.
            PlayJumpOrLand(false);                                         // The 'PlayJumpOrLand' method is perfomred, triggering our 'Jump & Land' event in FMOD to play. We also pass through it's parameter brackets the 'false' boolean value for our method to store inside a vaiable and use to play a landing sound with.
        }
        PreviosulyTouchingGround = PlayerTouchingGround; // Finally, we update the 'PreviosulyTouchingGround' variable by setting it to the value of whatever our 'PlayerTouchingGround' variable is. Then when the next frame is run, 'PlayerTouchingGround' will be updated, allowing us to compare it with 'PreviosulyTouchingGround', which still holds the value of it from the last frame.


        //This section determines when and how the PlayFootstep method should be told to trigger, thus playing the footstep event in FMOD.
        TimeTakenSinceStep += Time.deltaTime;                                // This adds whatever value Time.deltaTime is at to the 'TimeTakenToStep' float. Essentially tunring 'TimeTakenToStep' into a running timer that starts at 0 seconds. Remeber that 'Time.deltaTime' counts how long in seconds the last frame lasted for. 
        DistanceTravelled += (transform.position - PrevPos).magnitude;       // Every frame, the co-ordinates that the player is currently at is reduced by the value of the co-ordinates they were at during the previous frame. This gives us the length between the two points as a new set of co-ordinates (AKA a vector). That length is then tunred into a single number by '.magnitude' and then finally added onto the value of the DistanceTravelled float. Now we know how far the player has travlled!
        if (DistanceTravelled >= StepDistance + StepRandom)                  // If the distance the player has travlled is greater than or equal to the StepDistance plus the StepRandom, then we can perform our methods.
        {
            MaterialCheck();                                                 // The MaterialCheck method is perfomred. This checks for a material value and updates the 'F_MaterialValue' variable with for our 'PlayFootstep()' method to use.
            SpeedCheck();                                                    // The SpeedCheck method is performed. This checks for the time it took between this step and the last tot hen update the 'F_PlayerRunning' variable with for our 'PlayFootstep()' method to use.
            PlayFootstep();                                                  // The PlayFootstep method is performed and a footstep audio file from FMOD is played!
            StepRandom = Random.Range(0f, 0.5f);                             // Now that our footstep has been played, this will reset 'StepRandom' and give it a new random value between 0 and 0.5, in order to make the distance the player has to travel to hear a footstep different from what it previously was.
            DistanceTravelled = 0f;                                          // Since the player has just taken a step, we need to set the 'DistanceTravelled' float back to 0.
        }
        PrevPos = transform.position;                                        // To calculate where the player was during the last frame, we set 'PrevPos' to whatever positon the player is at during the end of the current frame, after the distance they'ev travleed has been calculated. This means that when this frame ends and the new one begins, the co-ordinates inside 'PrevPos' won't have changed, but will now represent where the player was during our last frame. 'Transform.postion' will then be updated at the very start of the new frame to hold whatever co-ordinates the player is now at.

    }


    void MaterialCheck() // This method when performed will find out what material our player is currenly on top of and will update the value of 'F_MaterialValue' accordingly, to represent that value.
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RayDistance))                                 // A raycast is fired down, from the position that the player is curenntly standing at, traveling as far as we decide to set the 'RayDistance' variable to. Infomration about the object it comes into contact with will then be stored inside the 'hit' variable for us to access.
        {
            if (hit.collider.gameObject.GetComponent<FMODStudioMaterialSetter>())                                    // Using the 'hit' varibale, we check to see if the raycast has hit a collider attached to a gameobject, that also has the 'FMODStudioMaterialSetter' script attached to it as a component...
            {
                F_MaterialValue = hit.collider.gameObject.GetComponent<FMODStudioMaterialSetter>().MaterialValue;    // ...and if it did, we then set our 'F_MaterialValue' varibale to match whatever value the 'MaterialValue' variable (which is inside the 'F_MaterialValue' varibale) is currently set to.
            }
            else                                                                                                     // Else if however, the player is standing on an object that doesn't have a 'FMODStudioMaterialSetter' script component for our raycast to find...
                F_MaterialValue = DefulatMaterialValue;                                                              // ...we then set 'F_MaterialValue' to match the value of 'DefulatMaterialValue'. 'DefulatMaterialValue' is given a value by the 'FMODStudioFootstepsEditor' script. This value represents whatever material we have selected as our 'DefulatMaterial' in the Unity Inspector tab.
        }
        else                                                                                                         // Else if however, the raycast can't find a collider attached to the object at all...
            F_MaterialValue = DefulatMaterialValue;                                                                  // Then again, we set 'F_MaterialValue' to match the value of 'DefulatMaterialValue'.
    }


    void SpeedCheck() // This method when performed will update the 'F_PlayerRunning' varibale, to find out if the player should be hearing heavy running footsteps.
    {
        if (TimeTakenSinceStep < StartRunningTime)                     // Remember that 'TimeTakenToStep' is a running timer. This method will only be performed as the player takes a step. So if this time is less than the value set to the 'StartRunningTime' variable, we can asume the player is moving fast enough to warrent a heavy running footstep to be played. So therefore...
            F_PlayerRunning = 1;                                       // ...we set 'F_PlayerRunning' to 1, which will be used in the 'PlayFootstep()' method to set our 'Speed Switch' parameter to 1 also.
        else                                                           // Else if however, the time inside 'TimeTakenToStep' is grater than 'StartRunningTime'...
            F_PlayerRunning = 0;                                       // ...we set 'F_PlayerRunning' to 0, which will be used in the 'PlayFootstep()' method to set our 'Speed Switch' parameter to 0 also.
        TimeTakenSinceStep = 0f;                                       // Finally, now that the player has taken the correct step, we reset our timer 'TimeTakenToStep' back to 0. Now th timer is tracking how much time has passed since the player took that last step.
    }


    void GroundedCheck() // This method will update the 'PlayerTouchingGround' variable, to find out if the player is currently touching the ground or not.
    {
         if (_motor.GroundingStatus.IsStableOnGround)                                                           // If that raycast find's a collider component at all...
            PlayerTouchingGround = true;                                            // ...we set the 'PlayerTouchingGround' variable to true, as this means the player must be standing on something.
        else                                                                        // Else if however, there is no collider component...
            PlayerTouchingGround = false;                                           // ...we set the 'PlayerTouchingGround' variable to false, as this means that nothing is colliding with the bottom of the capsule that is our players character, and therfore, the player is in the air.
    }


    void PlayFootstep() // When this method is performed, our footsteps event in FMOD will be told to play.
    {
        if (PlayerTouchingGround)                                                                                    // First we check to see the player is touching the ground.
        {
            FMOD.Studio.EventInstance Footstep = FMODUnity.RuntimeManager.CreateInstance(FootstepsEventPath);        // If they are, we create an FMOD event instance. We use the event path inside the 'FootstepsEventPath' variable to find the event we want to play.
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(Footstep, transform, GetComponent<Rigidbody>());     // Next that event instance is told to play at the location that our player is currently at.
            Footstep.setParameterByName(MaterialParameterName, F_MaterialValue);                                     // Before the event is played, we set the Material Parameter to match the value of the 'F_MaterialValue' variable.
            Footstep.setParameterByName(SpeedParameterName, F_PlayerRunning);                                        // We also set the Speed Parameter to match the value of the 'F_PlayerRunning' variable.
            Footstep.start();                                                                                        // We then play a footstep!.
            Footstep.release();                                                                                      // We also set our event instance to release straight after we tell it to play, so that the instance is released once the event had finished playing.
        }
    }


    void PlayJumpOrLand(bool F_JumpLandCalc) // When this method is performed our Jumping And Landing event in FMOD will be told to play. A boolean variable is also created inside it's parameter brackets to be used inside this method. This variable will hold whichever value we gave this method when we called it in the Update function.
    {
        FMOD.Studio.EventInstance Jl = FMODUnity.RuntimeManager.CreateInstance(JumpingEventPath);         // First we create an FMOD event instance. We use the event path inside the 'JumpingEventPath' variable to find the event we want to play.
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(Jl, transform, GetComponent<Rigidbody>());    // Next that event instance is told to play at the location that our player is currently at.
        Jl.setParameterByName(MaterialParameterName, F_MaterialValue);                                    // Before the event is played, we set the Material Parameter to match the value of the 'F_MaterialValue' variable.
        Jl.setParameterByName(JumpOrLandParameterName, F_JumpLandCalc ? 0f : 1f);                         // We also set the JumpOrLand Parameter to match the value of the 'F_JumpLandCalc' variable. Because this variable is a boolean and we need a float, we use the ? orpperator. It works exactly like an if statment. If 'F_JumpLandCalc' is true, our parameter get set to the first value that's to the left of the colon (0). Else if it's false, the value to the right of the colon (1) is set instead.
        Jl.start();                                                                                       // We then play our event, and the player either hear's a jumping or a landing sound!
        Jl.release();                                                                                     // We also set our event instance to release straight after we tell it to play, so that the instance is released once the event had finished playing.
    }
}
    //void Update()
    //{
    //    if (_motor.GroundingStatus.IsStableOnGround)
    //    {
    //        if (_motor.Velocity.y!=0 || _motor.Velocity.x !=0)
    //        {

    //            plaeySound = true;

    //        }
    //        else 
    //        {

    //            plaeySound = false;
    //        }
    //    }
    //    else
    //    {
    //       plaeySound = false;
    //    }


    //}
    //void CallFootsteps()
    //{
    //    if (plaeySound == true)
    //    {

    //        //FMODUnity.RuntimeManager.PlayOneShot();
    //    }

    //}
    //void Start()
    //{
    //    InvokeRepeating("CallFootsteps", 0, walkingSpeed);
    //}
    //void OnDisable()
    //{
    //    plaeySound = false;
    //}
