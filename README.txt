#########################################################
#####   	 VR Spatial Cue Experiment	    #####
#####					            #####
#####	      University of Hertfordshire 2016	    #####
#####		   Written By David Tree	    #####
#####						    #####
#########################################################

Experiment designed in collaboration with: Dr Kening Zhu, University College Hong Kong


Summary: 
Utilising 3 different Spatial Cues (Audio, Visual, Vibro-Haptic) this experiment tests a participants reaction time and their efficiency at achieving the target.

This experiment enables the testing of differing reaction speeds as well as providing a trace of rotations performed by the participant.


Unity Version: 3.5.2

Hardware Requirements for vibro haptics:
Arduino Uno with 8 x micro vibrating motors, connect via USB and put the COM port details into ExperimentRoom/Player/VibrohapticSystem/Com port variable.

Hardware Requirements for Virtual Reality:
Oculus DK2

Controls:
[SPACE] - start experiment
[R] - Replay training 


Usage:
After configuring Hardware requirments open Scenes/MainMenu and press play on unity's top bar,
on the computer enter the Participant number on main screen and press Start Experiment.

Instruct Participant to don the headset, headphones, VibroHaptic headband.
follow the onscreen instructions while providing the following suppliment to the participant.
Instruct user to look at the boxes that appear as quickly as possible, placing the reticle in the centre of the screen on the box until it disappears.  Repeat these steps until complete.


Output Data: /projectLocation/Data/

rotationLog_ _ParticipantNumber_cueType.xml
Description: stores a full log of all rotations (as quaternion) measured at each tick of the game clock.  

Results_ParticipantNumber.xml
Description: a summary report of entire experiment the time and sum angle achieved between each target.