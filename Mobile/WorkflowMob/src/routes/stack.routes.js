import React from 'react';
import { StyleSheet, Text, View, Image } from 'react-native';
import { NavigationContainer} from '@react-navigation/native';
import { createStackNavigator} from '@react-navigation/stack';
import { createBottomTabNavigator} from '@react-navigation/bottom-tabs';
import { useNavigation } from '@react-navigation/native';

import { getStyles } from './styles';
import { useTheme } from '../styles/themecontext'

import Inicio from '../../src/telas/Funcionario/Inicio';
import Login from '../../src/telas/Funcionario/Login';
import Home from '../../src/telas/Funcionario/Home';
import Tarefas from '../../src/telas/Funcionario/Tarefas';
import Equipes from '../../src/telas/Funcionario/Equipes';
import EquipeFuncionario from '../../src/telas/Funcionario/EquipeFuncionario';
import EquipeRanking from '../../src/telas/Funcionario/EquipeRanking';
import EquipeTarefas from '../../src/telas/Funcionario/EquipeTarefas';
import Ranking from '../../src/telas/Funcionario/Ranking';
import TarefaEnvio from '../../src/telas/Funcionario/TarefaEnvio';
import Configuracoes from '../../src/telas/Configuracoes'

import HomeAdm from '../../src/telas/Gestor/HomeAdm';
import CadastroEquipes from '../../src/telas/Gestor/CadastroEquipes';
import CadastroFuncionario from '../../src/telas/Gestor/CadastroFuncionario';
import CadastroTarefas from '../../src/telas/Gestor/CadastroTarefas';
import RankingAdm from '../../src/telas/Gestor/RankingAdm';
import RankingEstastistico from '../../src/telas/Gestor/RakingEstastistico';


import fonts from "../styles/fonts";
import {Ionicons} from '@expo/vector-icons';



const Tab = createBottomTabNavigator()


function Tabs(){
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const navigation = useNavigation();
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        headerStyle:   { backgroundColor: theme.background },
        headerTintColor: theme.text,
        headerTitle: 'WORKFLOW',
        headerTitleAlign: 'center',
        headerRight: () => (
          <Ionicons
            name="settings-outline"
            size={24}
            color={theme.text}
            style={styles.header}
            onPress={() => navigation.navigate('Configuracoes')}
          />
        ),
        tabBarIcon: ({ focused, color, size }) => {
          let iconName;
          if (route.name === 'Home') {
            iconName = 'home';
          } else if (route.name === 'Tarefas') {
            iconName = 'list';
          } else if (route.name === 'Equipes') {
            iconName = 'people';
          } else if (route.name === 'Ranking') {
            iconName = 'person';
          }
          const iconColor = focused
            ? theme.primary   
            : theme.text; 

          return <Ionicons name={iconName} size={size} color={iconColor} />;
        },

        tabBarActiveTintColor: theme.primary,   
        tabBarInactiveTintColor: theme.textSecondary, 

        tabBarStyle: {
          backgroundColor: theme.background,
        },
      })}
    >
      <Tab.Screen name="Home" component={Home} />
      <Tab.Screen name="Tarefas" component={Tarefas} />
      <Tab.Screen name="Equipes" component={Equipes} />
      <Tab.Screen name="Ranking" component={Ranking} />
    </Tab.Navigator>
  );
}

function TabsAdm(){  
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const navigation = useNavigation();
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        headerStyle:   { backgroundColor: theme.background },
        headerTintColor: theme.text,
        headerTitle: 'WORKFLOW',
        headerTitleAlign: 'center',
        headerRight: () => (
          <Ionicons
            name="settings-outline"
            size={24}
            color={theme.text}
            style={styles.header}
            onPress={() => navigation.navigate('Configuracoes')}
          />
        ),
        tabBarIcon: ({ focused, color, size }) => {
          let iconName;
          if (route.name === 'HomeAdm') {
            iconName = 'home';
          } else if (route.name === 'CadastroFuncionario') {
            iconName = 'person-circle-outline';
          } else if (route.name === 'CadastroTarefas') {
            iconName = 'list';
          } else if (route.name === 'CadastroEquipes') {
            iconName = 'people';
          } else if (route.name === 'RankingAdm') {
            iconName = 'person';
          }

          const iconColor = focused
            ? theme.primary   
            : theme.text; 

          return <Ionicons name={iconName} size={size} color={iconColor} />;
        },

        tabBarActiveTintColor: theme.primary,   
        tabBarInactiveTintColor: theme.textSecondary, 
        
        tabBarStyle: {
          backgroundColor: theme.background,
        },
      })}
    >
      <Tab.Screen name="HomeAdm" component={HomeAdm} />
      <Tab.Screen name="CadastroFuncionario" component={CadastroFuncionario} />
      <Tab.Screen name="CadastroTarefas" component={CadastroTarefas} />
      <Tab.Screen name="CadastroEquipes" component={CadastroEquipes} />
      <Tab.Screen name="RankingAdm" component={RankingAdm} />
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
          name="EquipeFuncionario"
          component={EquipeFuncionario}
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="EquipeTarefas"
          component={EquipeTarefas}
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="EquipeRanking"
          component={EquipeRanking} 
          options={{ headerShown: false }}
        />                
        <Stack.Screen
          name="TarefaEnvio"
          component={TarefaEnvio} 
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="Configuracoes"
          component={Configuracoes} 
          options={{ headerShown: false }}
        />
        <Stack.Screen
          name="HomeAdm"
          component={TabsAdm} 
          options={{ headerShown: false }}
        />   
        <Stack.Screen
          name="RankingEstastistico"
          component={RankingEstastistico} 
          options={{ headerShown: false }}
        />        
    
      </Stack.Navigator>
    </NavigationContainer>
  );
}

