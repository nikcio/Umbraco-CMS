export const name = 'Umbraco.Core.DataTypes';
export const extensions = [
	{
		name: 'Data Type Bundle',
		alias: 'Umb.Bundle.Language',
		type: 'bundle',
		js: () => import('./manifests.js'),
	},
];
