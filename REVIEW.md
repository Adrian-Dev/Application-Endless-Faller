# Project Review

## Applicant name - Adrian M.
---
### Summary
Overall I have a good feeling about the gameplay and I am satisfied with the current architecture design. I did not spend so much time in the gameplay mechanics or visual effects, but rather in designing and implementing a proper project and code structure, following a loosely coupled, modular guideline. Adding new functionality, such as new mechanics (player power ups, having platforms moving horizontaly, increase player speed, etc) should be much easier now compared to the first implementation in v1.0.

### Requirements subtasks 

(Not listed in any particular order. Bonus points are shown on _italics_)

- 1 
   - [x] The platforms should be randomly generated. No two playthroughs should have the same pattern.
   - _Time taken: around 1 hour_   
      - It took some time to prepare all different platforms set-up in Unity editor, but it was pretty straightforward. Platforms have been created as a set of different prefabs, which are then instantiated once at runtime and stored in a pool. A generic pooling system has been implemented to handle the pool resources, which is then wrapped by a platform manager to provide and release the required instances without destroying the object, hiding the actual implementation to the consumer (Level Controller).

- 2
   - [x] When I surpass my highscore, this should persist, and I should be able to see it in the UIs when I launch the game again. 
   - [x] When surpassing my high score, this should change on the menus. 
   - [x] _When surpassing my highscore, a new visual element should appear in the lost screen UI informing me of my achievement (like a text saying "You have a new highscore")._ 
   - [x] _When I'm about to surpass my highscore, a visual element should inform me of it (a different platform color, a particle effect, etc.)._ 
   - [x] _Don't use PlayerPrefs for highscore persistence._ 
   - _Time taken: around 1 hour and a half and two hours_  
      - Needed more time than estimated here (30m - 1h), having to prepare the high score interface and cleaning everything up, testing that it works and the implementation is robust. Also spent some time on researching how to persist data in Unity in different ways. 
      - Followed implementation in [Brackeys - SAVE & LOAD SYSTEM in Unity](https://www.youtube.com/watch?v=XOjd_qU2Ido/) for persisting high score. Having a saving system is something I had not worked on in Unity, and it was really helpful learning this resource. It also did not require me so much time for completing the test, which is Ok. I adapted the script to be a template, so the saving and loading system can work with a generic class.
      - To let the player know the current high score is about to be surpassed, a green colored platform will appear.
           
- 3
   - [x] On the game scene, the game functionality should be similar to the one in the gif. 
   - [x] When the character is out of screen, either because he couldn't fall fast enough or he fell before the next platform was available, the player should lose. 
   - [x] Every time the character goes down a floor, his score should increase. 
   - [x] _Don't use OnBecameInvisible for destroying the player._ 
   - _Time taken: Between half an hour and one hour_  
       - Made some boundary objects to demarcate the limit area of the playable game scene, so when the player (and platforms) reach them, a proper event will be triggered to handle it.
       
- 4
   - [x] On the main menu, I should see the current high score and when pressing play, change to the game scene. 
   - [x] When losing, a menu showing me my last score, my current highscore and the buttons: "Restart" and "Go back to main menu" should appear. The buttons should take the user back to the home scene and restart the level. 
   - [x] When I press escape, the game should pause, and I should have a menu with the option "Continue" and "Go back to main menu". The buttons should either continue the game or take the user back to the home scene. 
   - [x] _When restarting the game, don't reload the scene._ 
   - _Time taken: about two hours and a half and three hours_  
      -  Also needed more time than estimated here (30m - 1h), since I made a mayor change on the architecture to let it be as decoupled as possible, preparing the game flow, pause option, restart option, etc.
      - I used the same pause menu logic I implemented in [BullyYard](https://github.com/mostachostudios/TJ_game/) project.
      - **Note**: ‘Esc’ Key is a reserved key in Editor mode, so I used ‘M’ instead for developing,  but ‘Esc’ will work on build.
      - Changed 'Reset' for 'Restart' in LevelManager, since 'Reset' is a reserved function in Unity
      - Changed canvas scaler to force UI elements match with the current screen size, and updated the configuration to match a full 1080p resolution, both in Canvas and Project Settings (using native size)

- 5
   - [x] The platform spawn rate and speed should increase the longer the game lasts. 
   - [x] I should be able to change the initial spawn rate in an easy to modify way (scriptable object, config file, etc) 
   - _Time taken: around 1 hour_  
      - Needed some extra time to check how to properly store the initial spawn rate in a separeted file. Scriptable objects seem a good solution, but will only let change the value in development mode. I searched for a solution that would make possible to change the value once the game is built, but not sure if this was actually the purpose of this task. Anyway, this could be achieved using the same solution introduced for saving the high score, and it could be used for easily adding a difficulty level option (easy, medium, hard) according to the spawn rate value or initial platforms speed.
      - Spawn rate can be modified by the file located in the Config folder, and so can be modified the player speed and initial platforms speed.

- 6
   - [x] _Add some details to make the game scene prettier (lighting effects or particles, be creative)._ 
   - [x] _Have some smooth transitions between level transitions (fading/effects/etc)._ 
   -  _Time taken: about half an hour and 1 hour_
       - Particle effect in player triggered when colliding (game lost)
       - Post Process effect applied when pausing the game, so the user has a better feeling the game is not running
       - Fading screen effect when loading (and restarting) Game scene  

- 7
   - [x] _Write automated tests_ 
   -  _Time taken: about half an hour and 1 hour_
      - Having a loosely coupled code architecture makes writing unit tests quite straightforward. Still, some playtesting and integration testing are required in order to check the whole application. 
      
---
Some side notes: 
- As it can be seen in the UML diagram, the code has been structured so there are no loops between scripts dependecies. The level acts a manager who uses a set of different components to handle all the game flow. Only the LevelController script has a direct reference to the scene elements, acting as an Entry point of the application. It will compose objects as needed and inject the required dependencies to other instances. 

- The game was tested with several parameter values to make it fun (player speed, spawn rate, speed to be increased, etc)

