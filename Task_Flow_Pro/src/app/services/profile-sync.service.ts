import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfileSyncService {
  private profileUpdateSubject = new Subject<void>();

  // Observable for components to subscribe to
  profileUpdated$ = this.profileUpdateSubject.asObservable();

  // Call this method when profile data changes
  notifyProfileUpdate() {
    this.profileUpdateSubject.next();
  }
}
