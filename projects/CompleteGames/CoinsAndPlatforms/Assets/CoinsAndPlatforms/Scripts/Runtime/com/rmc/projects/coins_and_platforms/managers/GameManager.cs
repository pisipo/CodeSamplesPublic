/**
* Copyright (C) 2005-2014 by Rivello Multimedia Consulting (RMC).                    
* code [at] RivelloMultimediaConsulting [dot] com                                                  
*                                                                      
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the      
* "Software"), to deal in the Software without restriction, including  
* without limitation the rights to use, copy, modify, merge, publish,  
* distribute, sublicense, and#or sell copies of the Software, and to   
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:                                            
*                                                                      
* The above copyright notice and this permission notice shall be       
* included in all copies or substantial portions of the Software.      
*                                                                      
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,      
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF   
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
* IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR    
* OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
* ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
* OTHER DEALINGS IN THE SOFTWARE.                                      
*/
// Marks the right margin of code *******************************************************************


//--------------------------------------
//  Imports
//--------------------------------------
using UnityEngine;


//--------------------------------------
//  Namespace
//--------------------------------------
using com.rmc.exceptions;
using com.rmc.projects.coins_and_platforms.constants;
using System;
using com.rmc.projects.coins_and_platforms.components.super;


namespace com.rmc.projects.coins_and_platforms.managers
{
	
	//--------------------------------------
	//  Namespace Properties
	//--------------------------------------
	public enum GameOverReason
	{	
		WIN,
		LOSS
	}

	/// <summary>
	/// Game state.
	/// </summary>
	public enum GameState
	{
		NULL,
		MENU,
		GAME_BEGIN,
		LIVE_BEGIN,
		GAME,
		GAME_END
	}
	
	
	//--------------------------------------
	//  Class Attributes
	//--------------------------------------
	
	
	//--------------------------------------
	//  Class
	//--------------------------------------
	public class GameManager : MonoBehaviour 
	{
		
		//--------------------------------------
		//  Properties
		//--------------------------------------
		
		// GETTER / SETTER
		/// <summary>
		/// This is the gamestate
		/// 
		/// NOTE: Here we have a simple state machine
		/// 		* when a state is set, something may happen
		/// 		* if that is more than one line, we create a method for organization sake
		/// 		* the machine may immediatly advance state (otherwise some other scope will advance it)
		/// 
		/// 
		/// </summary>
		private GameState _gameState;
		public GameState gameState
		{
			get {
				return _gameState;
			}
			set {

				if (_gameState != value) {

					//SET
					_gameState = value;

					//KEEP TRACE
					//Debug.Log ("_gameState: " + _gameState);

					//SWITCH
					switch (_gameState)
					{
					case GameState.NULL:
						//NOT USED DIRECTLY
						break;
					case GameState.MENU:
						//TODO
						//MENU WILL DO THIS PART
						gameState = GameState.GAME_BEGIN;
						break;
					case GameState.GAME_BEGIN:
						_checkPoint_gameobject = _startWaypoint_gameobject;
						_doResetGUI();
						gameState = GameState.LIVE_BEGIN;
						break;
					case GameState.LIVE_BEGIN:
						_doResetPlayerOverall();
						//KEEP THIS
						gameState = GameState.GAME;
						break;
					case GameState.GAME:
						break;
					case GameState.GAME_END:
						_doDisablePlayer();
						break;
					default:
						#pragma warning disable 0162
						throw new SwitchStatementException();
						break;
						#pragma warning restore 0162

					}

				}
			}

		}

		/// <summary>
		/// The _score_float.
		/// </summary>
		private float _score_float;
		public float score
		{
			get {
				return _score_float;
			}
			set {
				_score_float = value;
				SimpleGameManager.Instance.guiManager.setScoreText (_score_float.ToString());
			}
			
		}


		/// <summary>
		/// The _lives_float.
		/// </summary>
		private float _lives_float;
		public float lives
		{
			get {
				return _lives_float;
			}
			set {
				_lives_float = value;
				Debug.Log ("lives: " + _lives_float);
				SimpleGameManager.Instance.guiManager.setLivesText (_lives_float.ToString());
			}
			
		}

		/// <summary>
		/// The _prompt message_string.
		/// </summary>
		private string _promptMessage_string;
		public string promptMessage
		{
			get {
				return _promptMessage_string;
			}
			set {
				_promptMessage_string = value;
				SimpleGameManager.Instance.guiManager.setPromptText (_promptMessage_string);
			}
			
		}

		
		/// <summary>
		/// The _check point_gameobject.
		/// </summary>
		private GameObject  _checkPoint_gameobject;
		public GameObject checkPoint
		{
			get {
				return _checkPoint_gameobject;
			}
			set {
				_checkPoint_gameobject = value;
			}
			
		}

		/// <summary>
		/// Gets the collected coin destination.
		/// 
		/// NOTE: We try 2 techniques. 
		/// 
		/// 	1)The first fails because the destination must be updated *as* the camera moves. TODO: Consider doing that.
		/// 
		/// 	2)The second is coin-relative, so it always works. Keep that for now.
		/// 
		/// 
		/// 
		/// 
		/// </summary>
		/// <returns>The collected coin destination.</returns>
		public Vector2 getCollectedCoinDestination (Transform aCurrentCoinPTransform)
		{




			//1) MOVE TO THE SCORE TEXT ON THE HUD
			/*
			Vector3 collectedCoinDestination_vector3 = Camera.main.ViewportToWorldPoint (new Vector3 (0.3f,.85f,0));
			//
			return new Vector2 (
				collectedCoinDestination_vector3.x, 
				collectedCoinDestination_vector3.y);
			*/


			//OR 2) MOVE 'UP' A LITTLE
			//1) MOVE TO THE SCORE TEXT ON THE HUD
			//
			return new Vector2 (
				aCurrentCoinPTransform.position.x, 
				aCurrentCoinPTransform.position.y + aCurrentCoinPTransform.lossyScale.y*8);

		}
		
		// PUBLIC
		
		// PUBLIC STATIC

		// PRIVATE
		/// <summary>
		/// The _last game over reason.
		/// </summary>
		private GameOverReason _lastGameOverReason;


		/// <summary>
		/// The _start waypoint_gameobject.
		/// </summary>
		private GameObject _startWaypoint_gameobject;

		/// <summary>
		/// The _start waypoint_gameobject.
		/// </summary>
		private GameObject _player_gameobject;


		// PRIVATE STATIC
		
		//--------------------------------------
		//  Methods
		//--------------------------------------	
		// PUBLIC
		
		///<summary>
		///	 Constructor
		///</summary>
		public GameManager ()
		{
			
			
		}
		
		/// <summary>
		/// Deconstructor
		/// </summary>
		~GameManager ( )
		{
			
			
		}
		
		///<summary>
		///	Use this for initialization
		///</summary>
		void Start () 
		{

			Debug.Log ("GM start()");
			_doSetBrittleReferences();

			//
			gameState = GameState.MENU;

		}


		/// <summary>
		/// _dos the set brittle references.
		/// 
		/// NOTE: For simplicity, this brittle approach is used instead of alternatives;
		/// 		* public transform references, set via dragging from hierarchy items
		/// 		* Inversion Of Control (StrangeIoC) where less GameObject-to-GameObject references are needed.
		/// 		* Manager dynamically spawning via Instantiate()
		/// 		* Etc...
		/// 
		/// 
		/// NOTE: We could shift this code to a newly created GUIManager. For now its fine.
		/// 
		/// 
		/// </summary>
		private void _doSetBrittleReferences()
		{
			//
			_startWaypoint_gameobject 	= _doThrowErrorIfNull (GameObject.Find (MainConstants.StartWaypoint)) as GameObject;
			_player_gameobject 			= _doThrowErrorIfNull (GameObject.Find (MainConstants.PlayerUnPrefab)) as GameObject;
			
		}


		/// <summary>
		/// Throws the error if null.
		/// </summary>
		/// <returns>The error if null.</returns>
		/// <param name="aToCheck_object">A to check_object.</param>
		private UnityEngine.Object _doThrowErrorIfNull(UnityEngine.Object aToCheck_object)
		{
			
			if (aToCheck_object == null) {
				throw new Exception ("Must not be null : " + aToCheck_object);
			}

			return aToCheck_object;
			
		}
		
		
		///<summary>
		///	Called once per frame
		///</summary>
		void Update () 
		{
			
		}
		
		// PUBLIC

		/// <summary>
		/// Dos the kill player.
		/// </summary>
		public void doKillPlayer ()
		{
			lives--;
			//
			if (lives > 0) {
				gameState = GameState.LIVE_BEGIN;
			} else {
				doGameOver (GameOverReason.LOSS);
			}
		}

		/// <summary>
		/// Dos the reward score.
		/// 
		/// NOTE: Handles "100 points" floating text too
		/// 
		/// </summary>
		/// <param name="aScore_float">A score_float.</param>
		public void doRewardScore (float aScore_float, Vector3 aPosition_vector3)
		{

			score += aScore_float;
			SimpleGameManager.Instance.doInstantiateDynamicPrefab (MainConstants.RisingPointsPrefab, aPosition_vector3);
		}

		/// <summary>
		/// Dos the restart game.
		/// </summary>
		public void doRestartGame () 
		{

			gameState = GameState.NULL;
			gameState = GameState.MENU;

			//FOR DEBUGGING, RELOAD CURRENT SCENE
			//SimpleGameManager.Instance.doRestartScene();

			//FOR PRODUCTION, RELOAD AND SHOW MENU
			SimpleGameManager.Instance.doRestartApplication();

			
		}


		/// <summary>
		/// Dos the game over.
		/// </summary>
		/// <param name="aGameOverReason">A game over reason.</param>
		public void doGameOver (GameOverReason aGameOverReason) 
		{
			_lastGameOverReason = aGameOverReason;
			_doGameOver_Part1();

		}


		/// <summary>
		/// _dos the game over immediate.
		/// </summary>
		private void _doGameOver_Part1()
		{
			//Time.timeScale = .1f;
			gameState = GameState.GAME_END;

			//
			switch (_lastGameOverReason) {
			case GameOverReason.WIN:
				promptMessage = MainConstants.PROMPT_GAME_OVER_WIN;
				SimpleGameManager.Instance.audioManager.doPlaySound (AudioClipType.GAME_OVER_WIN);

				break;
			case GameOverReason.LOSS:
				promptMessage = MainConstants.PROMPT_GAME_OVER_LOSS;
				SimpleGameManager.Instance.audioManager.doPlaySound (AudioClipType.GAME_OVER_LOSS);
				break;
			default:
				#pragma warning disable 0162
				throw new SwitchStatementException();
				break;
				#pragma warning restore 0162
			}


			//
			PlayerInputComponent playerInputComponent = _player_gameobject.GetComponent<PlayerInputComponent>();
			playerInputComponent.doResetPhysicsAndAnimation();
			//
			CharacterController2D _characterController2D = _player_gameobject.GetComponent<CharacterController2D>();
			_characterController2D.enabled = false;
			//playerInputComponent.enabled = false;
			//playerInputComponent.enabled = false;


			Invoke ("_doGameOver_Part2", 0.25f);
		}


		/// <summary>
		/// _dos the game over after pause.
		/// </summary>
		private void _doGameOver_Part2()
		{

			//cleanup anything?

		}

		/// <summary>
		/// _dos the reset GU.
		/// </summary>
		/// todo: order these methods better
		private void _doResetGUI() 
		{ 
			score = 0;
			lives = 2;
			promptMessage = "";

		}

		/// <summary>
		/// _dos the reset player.
		/// </summary>
		private void _doResetPlayerOverall() 
		{

			_doResetPlayer_Position();

			//
			PlayerInputComponent playerInputComponent = _player_gameobject.GetComponent<PlayerInputComponent>();
			playerInputComponent.doResetPhysicsAndAnimation();

		}

		/// <summary>
		/// _dos the reset player.
		/// </summary>
		private void _doResetPlayer_Position() 
		{
			
			//SET TO THE LEFT OF THE WAYPOINT FLAG
			_player_gameobject.transform.position 		= new Vector3 
				(
					_checkPoint_gameobject.transform.position.x - _player_gameobject.transform.localScale.x,
					_checkPoint_gameobject.transform.position.y + 8,
					_checkPoint_gameobject.transform.position.z
					);
		}




		/// <summary>
		/// _dos the disable player.
		/// </summary>
		private void _doDisablePlayer ()
		{
			CharacterController2D characterController2D = _player_gameobject.GetComponent<CharacterController2D>();
			characterController2D.velocity = Vector2.zero;
			characterController2D.enabled = false;

		}


		//PRIVATE
		
		//--------------------------------------
		//  Events
		//--------------------------------------
	}
}
