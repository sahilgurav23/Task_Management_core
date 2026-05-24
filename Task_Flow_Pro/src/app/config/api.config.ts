import { environment } from '../../environments/environment';

export class ApiConfig {
  private static baseUrl = environment.apiUrl;

  static get endpoints() {
    return {
      // Auth endpoints
      login: `${this.baseUrl}/api/Login/validate`,
      
      // Add more endpoints here as needed
      // user: `${this.baseUrl}/api/User`,
      // admin: `${this.baseUrl}/api/Admin`,
      // etc.
    };
  }
}
