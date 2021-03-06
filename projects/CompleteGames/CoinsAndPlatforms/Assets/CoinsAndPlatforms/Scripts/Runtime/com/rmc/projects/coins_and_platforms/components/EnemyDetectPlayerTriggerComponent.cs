﻿/**
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
using com.rmc.projects.coins_and_platforms.components.core;
using com.rmc.projects.coins_and_platforms.constants;
using com.rmc.projects.coins_and_platforms.managers;
using UnityEngine;

//--------------------------------------
//  Namespace
//--------------------------------------
namespace com.rmc.projects.coins_and_platforms.components.super
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
	[RequireComponent (typeof (EnemyAIComponent))]
	public class EnemyDetectPlayerTriggerComponent : SuperTriggerComponent 
	{
		
		
		//--------------------------------------
		//  Properties
		//--------------------------------------
		
		// GETTER / SETTER
		
		// PUBLIC
		
		/// <summary>
		/// The _enemy AI component.
		/// </summary>
		private EnemyAIComponent _enemyAIComponent;
		
		
		// PRIVATE STATIC
		
		//--------------------------------------
		//  Methods
		//--------------------------------------	
		// PUBLIC
		
		///<summary>
		///	 Constructor
		///</summary>
		public EnemyDetectPlayerTriggerComponent ()
		{
			
			
		}
		
		/// <summary>
		/// Deconstructor
		/// </summary>
		~EnemyDetectPlayerTriggerComponent ( )
		{
			
			
		}
		
		
		
		
		///<summary>
		///	Use this for initialization
		///</summary>
		void Start () 
		{
			_wasTriggered = false;
			_enemyAIComponent = GetComponent <EnemyAIComponent>();
		}
		
		
		
		/// <summary>
		/// Called once per frame
		/// </summary>
		void Update()
		{
			
		}
		
		// PUBLIC
		/// <summary>
		/// Dos the refresh 
		/// </summary>
		public void doRefreshEnemy ()
		{
			_wasTriggered = false;
			
		}
		
		// PUBLIC STATIC
		
		// PRIVATE
		/// <summary>
		/// _dos the trigger waypoing.
		/// 
		/// NOTE: We crudely evaluate victory here. Todo: More checks could be added (player velocity.y)
		/// 
		/// </summary>
		private void _doTriggerWaypoint (bool isEnemyVictorious_boolean)
		{
			_wasTriggered = true;
			Invoke ("doRefreshEnemy",1f);
			//
			if (isEnemyVictorious_boolean) {
				SimpleGameManager.Instance.audioManager.doPlaySound (AudioClipType.ENEMY_KILLS_PLAYER);
				SimpleGameManager.Instance.gameManager.doKillPlayer();
			} else {
				SimpleGameManager.Instance.audioManager.doPlaySound (AudioClipType.PLAYER_KILLS_ENEMY);
				_enemyAIComponent.doKillEnemy();
			}
			
		}
		
		// PRIVATE STATIC
		
		// PRIVATE COROUTINE
		
		// PRIVATE INVOKE
		
		//--------------------------------------
		//  Events
		//--------------------------------------
		/// <summary>
		/// Raises the trigger enter2 d event.
		/// </summary>
		/// <param name="collider2D">Collider2 d.</param>
		public void OnTriggerEnter2D (Collider2D collider2D)
		{
			if (collider2D.gameObject.tag == MainConstants.PLAYER_TAG) {
				if (!_wasTriggered) {
					CharacterController2D _characterController2D = collider2D.gameObject.GetComponent<CharacterController2D>();
					_doTriggerWaypoint(_characterController2D.isGrounded);
				}
			}
			
		}
		
	}
}