import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../model/user';
import { LoginModel } from '../model/login';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor(private http: HttpClient) {   }
  Login(login:LoginModel) 
  {
    return this.http.post<User>('https://localhost:7289/Session',login ,{withCredentials:true});
  }
  Logout() {
    return this.http.delete("https://localhost:7289/Session",{withCredentials:true});
  }
  GetSession() {
    return this.http.get<User>("https://localhost:7289/Session",{withCredentials:true});
    }

}
