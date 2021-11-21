import { Component } from '@angular/core';
import { CartFacade } from '@shoppit/shared/state';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable, Subscription } from 'rxjs';
import { environment } from '../environments/environment';

@Component({
  selector: 'shoppit-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  authSvcEventSubscription$: Subscription | undefined;

  isLoggedIn = false;
  collapsed = true;
  totalCartItems$: Observable<number>;

  constructor(private authSvc: OAuthService, private cartState: CartFacade) {
    authSvc.configure({
      issuer: environment.authentication.issuer,
      redirectUri: window.location.origin,
      clientId: environment.authentication.clientId,
      responseType: 'code',
      scope: environment.authentication.scopes,
      showDebugInformation: !environment.production,
    });
    authSvc.loadDiscoveryDocumentAndTryLogin();
    authSvc.setupAutomaticSilentRefresh();
    this.totalCartItems$ = cartState.totalCartItems$;
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
