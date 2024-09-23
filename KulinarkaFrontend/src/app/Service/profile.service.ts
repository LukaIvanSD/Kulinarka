import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Profile } from '../model/profile';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private http:HttpClient) { }
  GetProfile() :Observable<Profile>{
    return this.http.get<Profile>("https://localhost:7289/Profile",{withCredentials:true});
  }
}
