import { Component } from '@angular/core';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';
import { Subscription } from 'rxjs';

export const authCodeFlowConfig: AuthConfig = {
  // Url of the Identity Provider
  issuer: 'https://localhost:5001/',

  // URL of the SPA to redirect the user to after login
  redirectUri: window.location.origin,

  // The SPA's id. The SPA is registerd with this id at the auth-server
  // clientId: 'server.code',
  clientId: 'storefront',

  // Just needed if your auth server demands a secret. In general, this
  // is a sign that the auth server is not configured with SPAs in mind
  // and it might not enforce further best practices vital for security
  // such applications.
  // dummyClientSecret: 'secret',

  responseType: 'code',

  // set the scope for the permissions the client should request
  // The first four are defined by OIDC.
  // Important: Request offline_access to get a refresh token
  // The api scope is a usecase specific one
  scope: 'openid  offline_access api',

  showDebugInformation: true,
};

@Component({
  selector: 'shoppit-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  authSvcEventSubscription$: Subscription | undefined;

  isLoggedIn = false;
  collapsed = false;

  constructor(private authSvc: OAuthService) {
    authSvc.configure(authCodeFlowConfig);
    authSvc.loadDiscoveryDocumentAndTryLogin();
  }

  ngOnInit() {
    this.authSvcEventSubscription$ = this.authSvc.events.subscribe(
      (ev) => (this.isLoggedIn = this.authSvc.hasValidAccessToken())
    );
  }

  toggleNavbar() {
    this.collapsed = !this.collapsed;
  }

  login() {
    this.authSvc.initLoginFlow();
  }

  logout() {
    this.authSvc.logOut();
  }

  ngOnDestroy(): void {
    this.authSvcEventSubscription$?.unsubscribe();
  }
}
