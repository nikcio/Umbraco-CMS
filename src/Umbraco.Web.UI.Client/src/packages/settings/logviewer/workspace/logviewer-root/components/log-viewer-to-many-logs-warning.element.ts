import { css, html, LitElement , customElement } from '@umbraco-cms/backoffice/external/lit';

@customElement('umb-log-viewer-to-many-logs-warning')
export class UmbLogViewerToManyLogsWarningElement extends LitElement {
	render() {
		return html`<uui-box id="to-many-logs-warning">
			<h3>Unable to view logs</h3>
			<p>Today's log file is too large to be viewed and would cause performance problems.</p>
			<p>If you need to view the log files, narrow your date range or try opening them manually.</p>
		</uui-box>`;
	}

	static styles = [
		css`
			:host {
				text-align: center;
			}
		`,
	];
}

declare global {
	interface HTMLElementTagNameMap {
		'umb-log-viewer-to-many-logs-warning': UmbLogViewerToManyLogsWarningElement;
	}
}
