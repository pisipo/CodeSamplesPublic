/**
 * Copyright (C) 2005-2014 by Rivello Multimedia Consulting (RMC).                    
 * code [at] RivelloMultimediaConsulting [dot] com                                                  
 *                                                                      
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the      
 * "Software"), to deal in the Software without restriction, including  
 * without limitation the rights to use, copy, modify, merge, publish,  
 * distribute, sublicense, and#or sell copies of the Software, and to   
 * permit persons to whom the Software is furn
 * 
 * ished to do so, subject to
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
using strange.extensions.mediation.impl;
using com.rmc.projects.paddle_soccer.mvcs.view.ui;
using com.rmc.projects.paddle_soccer.mvcs.controller.signals;
using com.rmc.projects.paddle_soccer.mvcs.model.vo;
using com.rmc.projects.paddle_soccer.mvcs.model;
using com.rmc.exceptions;

//--------------------------------------
//  Namespace
//--------------------------------------
using com.rmc.projects.paddle_soccer.components;


namespace com.rmc.projects.paddle_soccer.mvcs.view.mediators
{
	
	//--------------------------------------
	//  Namespace Properties
	//--------------------------------------
	
	
	//--------------------------------------
	//  Class Attributes
	//--------------------------------------
	
	
	//--------------------------------------
	//  Class
	//--------------------------------------
	public class SoccerBallUIMediator : Mediator
	{
		
		//--------------------------------------
		//  Properties
		//--------------------------------------
		
		/// <summary>
		/// Gets or sets the view.
		/// </summary>
		/// <value>The view.</value>
		[Inject]
		public SoccerBallUI view 	{ get; set;}
		
		/// <summary>
		/// Gets or sets the sound play signal.
		/// </summary>
		/// <value>The sound play signal.</value>
		[Inject]
		public SoundPlaySignal soundPlaySignal { get; set;}

		
		/// <summary>
		/// Gets or sets the game state changed signal.
		/// </summary>
		/// <value>The game state changed signal.</value>
		[Inject]
		public GameStateChangedSignal gameStateChangedSignal {set; get;}
		


		// PUBLIC
		
		
		// PUBLIC STATIC
		
		// PRIVATE
		
		// PRIVATE STATIC
		
		//--------------------------------------
		//  Methods
		//--------------------------------------
		/// <summary>
		/// Raises the register event.
		/// </summary>
		public override void OnRegister()
		{
			//
			view.init();
			gameStateChangedSignal.AddListener (_onGameStateChangedSignal);
			view.uiCollisionEnter2DSignal.AddListener (_onUICollisionEnter2DSignal);
			
		}
		
		/// <summary>
		/// Raises the remove event.
		/// </summary>
		public override void OnRemove()
		{
			//
			gameStateChangedSignal.RemoveListener (_onGameStateChangedSignal);
			view.uiCollisionEnter2DSignal.RemoveListener (_onUICollisionEnter2DSignal);
		}
		
		
		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update()
		{

		}
		
		
		//	PUBLIC
		
		
		// PRIVATE
		
		// PRIVATE STATIC
		
		// PRIVATE COROUTINE
		
		// PRIVATE INVOKE
		
		//--------------------------------------
		//  Events
		//--------------------------------------
		/// <summary>
		/// _ons the user interface collision enter2 D signal.
		/// </summary>
		/// <param name="aTag_string">A tag_string.</param>
		private void _onUICollisionEnter2DSignal (GameObject aCollidedWith_gameobject) 
		{

			//WHAT DID WE HIT?
			switch (aCollidedWith_gameobject.tag){
			case CPUPaddleUI.TAG:
			case PlayerPaddleUI.TAG:
				//
				PaddleComponent paddleComponent = aCollidedWith_gameobject.GetComponent<PaddleComponent>();
				view.doHandleCollisionWithPaddle (paddleComponent.velocity);
				//
				soundPlaySignal.Dispatch (new SoundPlayVO (SoundType.PADDLE_HIT));
				break;


			case BoundaryComponent.TAG:

				//WHICH TYPE OF BOUNDARY WAS HIT?
				BoundaryComponent boundaryComponent = aCollidedWith_gameobject.GetComponent<BoundaryComponent>();
				//
				switch (boundaryComponent.boundaryType){
				case BoundaryType.LeftGoal:
					soundPlaySignal.Dispatch (new SoundPlayVO (SoundType.GAME_OVER_WIN));
					break;
				case BoundaryType.RightGoal:
					soundPlaySignal.Dispatch (new SoundPlayVO (SoundType.GAME_OVER_LOSS));
					break;
				case BoundaryType.None:
					soundPlaySignal.Dispatch (new SoundPlayVO (SoundType.PADDLE_HIT));
					break;
				default:
					#pragma warning disable 0162
					throw new SwitchStatementException();
					break;
					#pragma warning restore 0162    
				}
				break;

			}

		}


		/// <summary>
		/// When the game state changed signal.
		/// </summary>
		/// <param name="aGameState">A game state.</param>
		private void _onGameStateChangedSignal (GameState aGameState)
		{

			//
			//Debug.Log ("!!!_onGameStateChangedSignal()");


			//todo:change to
			//if (aGameState == GameState.ROUND_DURING_CORE_GAMEPLAY) {
			if (aGameState == GameState.ROUND_DURING_CORE_GAMEPLAY ) {
				view.isRunningUpdate = true;
				view.doResetToStartingPosition();
				view.doGiveStartingPush();
			} else {
				view.isRunningUpdate = false;
			}
			
		}


		
	}
}
