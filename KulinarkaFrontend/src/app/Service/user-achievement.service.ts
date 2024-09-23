import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserAchievement } from '../model/userAchievement';
@Injectable({
  providedIn: 'root'
})
export class UserAchievementService {

  constructor(private http:HttpClient) {
   }
   GetUserAchievements() :Observable<UserAchievement[]>{
  return this.http.get<UserAchievement[]>("https://localhost:7289/UserAchievement",{withCredentials:true}); 
  }
}
