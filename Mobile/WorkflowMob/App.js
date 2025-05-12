import React from 'react';
import { StyleSheet, Text, View, Image } from 'react-native';
import { NavigationContainer} from '@react-navigation/native';
import { createStackNavigator} from '@react-navigation/stack';
import { createBottomTabNavigator} from '@react-navigation/bottom-tabs';

import Inicio from './src/telas/Inicio';
import Login from './src/telas/Login';
import Home from './src/telas/Home';
import Tarefas from './src/telas/Tarefas';
import Equipes from './src/telas/Equipes';
import Raking from './src/telas/Raking';
import TarefaEnvio from './src/telas/TarefaEnvio';
import {Ionicons} from '@expo/vector-icons';

const Tab = createBottomTabNavigator()


function Tabs(){
  return (
    <Tab.Navigator
    screenOptions={({ route }) => ({
      headerTitle: 'WORKFLOW',
      headerTitleAlign: 'center',
      headerRight: () => (
        <Ionicons
          name="settings-outline"
          size={24}
          color="#3f64c7"
          style={{ marginRight: 15 }}
          onPress={() => navigation.navigate('Configuracoes')}
        />
      ),
      
      tabBarIcon: ({ focused, color, size }) => {
        let iconName;
        if (route.name === 'Home') {
          iconName = focused
            ? 'home': 'home';
        } else if (route.name === 'Tarefas') {
          iconName = focused ? 'list' : 'list';
        }else if (route.name === 'Equipes') {
          iconName = focused ? 'people' : 'people';
        }else if (route.name === 'Raking') {
          iconName = focused ? 'person' : 'person';
        }

        
        return <Ionicons name={iconName} size={size} color={color} />;
      },
    })}
    tabBarOptions={{
      activeTintColor: '#3f64c7',
      inactiveTintColor: 'gray',      
    }}    
    >  
      <Tab.Screen name= "Home" component={Home}></Tab.Screen>
      <Tab.Screen name= "Tarefas" component={Tarefas}></Tab.Screen>
      <Tab.Screen name= "Equipes" component={Equipes}></Tab.Screen>
      <Tab.Screen name= "Raking" component={Raking}></Tab.Screen>
    </Tab.Navigator>
  );
}

export default function App() {

  const Stack = createStackNavigator();
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="Inicio">
        <Stack.Screen
          name="Inicio"
          component={Inicio}
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="Login"
          component={Login}
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="Home"
          component={Tabs} 
          options={{ headerShown: false }}
        />        
        <Stack.Screen
          name="Equipes"
          component={Tabs}
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="Tarefas"
          component={Tabs} 
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="Raking"
          component={Tabs}
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="TarefaEnvio"
          component={TarefaEnvio} 
          options={{ headerShown: false }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
