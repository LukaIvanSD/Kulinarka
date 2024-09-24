import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Profile } from '../model/profile';
import { Observable } from 'rxjs';
import { User } from '../model/user';
import { UserInfo } from '../model/profile';


@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private http:HttpClient) { }
  GetProfile() :Observable<Profile>{
    return this.http.get<Profile>("https://localhost:7289/Profile",{withCredentials:true});
  }
  UpdateUserInfo(userInfo :UserInfo):Observable<User>{
    return this.http.put<User>('https://localhost:7289/Profile/ChangeInfo',userInfo,{withCredentials:true});
  }
  ChangePassword(oldPassword:string,newPassword:string):Observable<User>{
    return this.http.post<User>("https://localhost:7289/Profile/ChangePassword",{oldPassword,newPassword},{withCredentials:true});
  }
  ChangePicture(picture:FormData):Observable<string>{
    return this.http.put<string>("https://localhost:7289/Profile/ChangePicture",picture,{withCredentials:true});
  }
}
