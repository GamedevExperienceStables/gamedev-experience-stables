using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using KinematicCharacterController;

[RequireComponent(typeof(KinematicCharacterMotor))]
public class AudioController : MonoBehaviour
{
    private KinematicCharacterMotor _motor;
   
    [Header("FMOD Settings")]
    [SerializeField] private FMODUnity.EventReference FootstepsEventPath;    
    [SerializeField] private FMODUnity.EventReference JumpingEventPath;      
    [SerializeField] private string MaterialParameterName;                      
    [SerializeField] private string SpeedParameterName;                         
    [SerializeField] private string JumpOrLandParameterName;                    
    [Header("Playback Settings")]
    [SerializeField] private float StepDistance = 2.0f;                         
    [SerializeField] private float RayDistance = 1.2f;                          
    [SerializeField] private float StartRunningTime = 0.3f;                     
    [SerializeField] private string JumpInputName;                              
    public string[] MaterialTypes;                                              
    [HideInInspector] public int DefulatMaterialValue;                          // This will be told by the 'FMODStudioFootstepsEditor' script which Material has been set as the defualt. It will then store the value of that Material for outhis script to use. This cannot be changed in the Editor, but a drop down menu created by the 'FMODStudioFootstepsEditor' script can.
   
    private float StepRandom;                                                   
    private Vector3 PrevPos;                                                    
    private float DistanceTravelled;                                           
    
    private RaycastHit hit;                                                     
    private int F_MaterialValue;                                                
    
    private bool PlayerTouchingGround;                                          
	private bool PreviosulyTouchingGround;                                      
    
    private float TimeTakenSinceStep;                                           
    private int F_PlayerRunning;                                                

   
   
    
    private void Awake()
    {
        _motor = GetComponent<KinematicCharacterMotor>();
       
    }
    void Start() 
    {
        StepRandom = Random.Range(0f, 0.5f);        
        PrevPos = transform.position;               
    }
    void Update() 
    {

        
          GroundedCheck();                                                  
		  if (PlayerTouchingGround && Input.GetButtonDown(JumpInputName))    
        {
            MaterialCheck();                                               
            PlayJumpOrLand(true);                                          
        }
        if (!PreviosulyTouchingGround && PlayerTouchingGround)             
        {
            MaterialCheck();                                               
            PlayJumpOrLand(false);                                         
			}
        PreviosulyTouchingGround = PlayerTouchingGround; 


        //This section determines when and how the PlayFootstep method should be told to trigger, thus playing the footstep event in FMOD.
        TimeTakenSinceStep += Time.deltaTime;                                
        DistanceTravelled += (transform.position - PrevPos).magnitude;       
		if (DistanceTravelled >= StepDistance + StepRandom)                  
			{
            MaterialCheck();                                                 
			SpeedCheck();                                                    
			PlayFootstep();                                                  
			StepRandom = Random.Range(0f, 0.5f);                             
			DistanceTravelled = 0f;                                          
			}
        PrevPos = transform.position;                                        
		
    }


    void MaterialCheck() // This method when performed will find out what material our player is currenly on top of and will update the value of 'F_MaterialValue' accordingly, to represent that value.
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RayDistance))                                 
			{
            if (hit.collider.gameObject.GetComponent<FMODStudioMaterialSetter>())                                   
				{
                F_MaterialValue = hit.collider.gameObject.GetComponent<FMODStudioMaterialSetter>().MaterialValue;   
				}
            else                                                                                                    
				F_MaterialValue = DefulatMaterialValue;                                                            
			}
        else                                                                                                        
			F_MaterialValue = DefulatMaterialValue;                                                                  
		}


    void SpeedCheck() // This method when performed will update the 'F_PlayerRunning' varibale, to find out if the player should be hearing heavy running footsteps.
    {
        if (TimeTakenSinceStep < StartRunningTime)                     
			F_PlayerRunning = 1;                                      
		else                                                           
			F_PlayerRunning = 0;                                       
		TimeTakenSinceStep = 0f;                                       
		}


    void GroundedCheck() // This method will update the 'PlayerTouchingGround' variable, to find out if the player is currently touching the ground or not.
    {
         if (_motor.GroundingStatus.IsStableOnGround)                                                           
            PlayerTouchingGround = true;                                            
        else                                                                       
            PlayerTouchingGround = false;                                          
		}


    void PlayFootstep() // When this method is performed, our footsteps event in FMOD will be told to play.
    {
        if (PlayerTouchingGround)                                                                                    
        {
            FMOD.Studio.EventInstance Footstep = FMODUnity.RuntimeManager.CreateInstance(FootstepsEventPath);        
			FMODUnity.RuntimeManager.AttachInstanceToGameObject(Footstep, transform, GetComponent<Rigidbody>());     
			Footstep.setParameterByName(MaterialParameterName, F_MaterialValue);                                     
			Footstep.setParameterByName(SpeedParameterName, F_PlayerRunning);                                        
			Footstep.start();                                                                                       
            Footstep.release();                                                                                      
			}
    }


    void PlayJumpOrLand(bool F_JumpLandCalc) // When this method is performed our Jumping And Landing event in FMOD will be told to play. A boolean variable is also created inside it's parameter brackets to be used inside this method. This variable will hold whichever value we gave this method when we called it in the Update function.
    {
        FMOD.Studio.EventInstance Jl = FMODUnity.RuntimeManager.CreateInstance(JumpingEventPath);         
		FMODUnity.RuntimeManager.AttachInstanceToGameObject(Jl, transform, GetComponent<Rigidbody>());    
		Jl.setParameterByName(MaterialParameterName, F_MaterialValue);                                    
		Jl.setParameterByName(JumpOrLandParameterName, F_JumpLandCalc ? 0f : 1f);                         
		Jl.start();                                                                                       
		Jl.release();                                                                                    
		}
}
    