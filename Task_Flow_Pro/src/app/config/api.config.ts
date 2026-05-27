import { environment } from '../../environments/environment';

export class ApiConfig {
  private static baseUrl = environment.apiUrl;

  static get endpoints() {
    return {
      // Auth endpoints
      login: `${this.baseUrl}/api/Login/validate`,
      
      // Profile endpoints
      profile: `${this.baseUrl}/api/ProfileControllercs`,
      
      // Task endpoints
      task: `${this.baseUrl}/api/Task`,
      
      // Add more endpoints here as needed
      // user: `${this.baseUrl}/api/User`,
      // admin: `${this.baseUrl}/api/Admin`,
      // etc.
    };
  }
}
