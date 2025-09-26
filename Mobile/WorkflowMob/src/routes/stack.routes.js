import React from 'react';

import { StyleSheet, Text, View, Image } from 'react-native';
import { NavigationContainer} from '@react-navigation/native';
import { createStackNavigator} from '@react-navigation/stack';
import { createDrawerNavigator } from '@react-navigation/drawer';
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
import EquipesAdm from '../../src/telas/Gestor/EquipesAdm';
import EditEquipe from '../../src/telas/Gestor/EditEquipe';

import fonts from '../styles/fonts';
import {Ionicons} from '@expo/vector-icons';


const Tab = createBottomTabNavigator()

function Tabs({route}){
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario || null;
  const equipe = route.params?.equipe || null;

  const navigation = useNavigation();
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        headerStyle:   { backgroundColor: theme.background },
        headerTintColor: theme.text,
        headerTitleAlign: 'center',
        headerTitle: () => (
          <Image
            source={theme.logo}
            style={{ width: 170, height: 170, marginBottom:10, }}
          />
        ),
        headerRight: () => (
          <Ionicons
            name="settings-outline"
            size={24}
            color={theme.text}
            style={styles.header}
            onPress={() => navigation.navigate('Configuracoes', { usuario, equipe})}
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
      <Tab.Screen name="Home" component={Home} initialParams={{ usuario }}  />
      <Tab.Screen name="Tarefas" component={Tarefas} initialParams={{ usuario }}  />
      <Tab.Screen name="Equipes" component={Equipes} initialParams={{ usuario }}  />
      <Tab.Screen name="Ranking" component={Ranking} initialParams={{ usuario }}  />
    </Tab.Navigator>
  );
}

function DrawerAdm({route, navigation}){  
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const Drawer = createDrawerNavigator();
  const usuario = route.params?.usuario || null;
  const equipe = route.params?.equipe || null;


  return (
    <Drawer.Navigator
      screenOptions={({ route }) => ({
        headerStyle:   { backgroundColor: theme.background },
        headerTintColor: theme.text,
        headerTitleAlign: 'center',
        headerTitle: () => (
          <Image
            source={theme.logo}
            style={{ width: 170, height: 170, marginBottom:10,}}
          />
        ),
        headerRight: () => (
          <Ionicons
            name="settings-outline"
            size={24}
            color={theme.text}
            style={styles.header}
            onPress={() => navigation.navigate('Configuracoes', { usuario, equipe })}
          />
        ),

        drawerIcon: ({ focused, size, color }) => {
          let iconName;
          if (route.name === 'HomeAdm') {
            iconName = 'home';
          } else if (route.name === 'CadastroFuncionario') {
            iconName = 'person-circle-outline';
          } else if (route.name === 'CadastroTarefas') {
            iconName = 'list';
          } else if (route.name === 'CadastroEquipes') {
            iconName = 'person-add-outline';
          } else if (route.name === 'EquipesAdm') {
            iconName = 'people-sharp';
          } else if (route.name === 'RankingAdm') {
            iconName = 'trophy';
          }

          const iconColor = focused
            ? theme.primary   
            : theme.text; 

          return <Ionicons name={iconName} size={size} color={iconColor} />;
        },

      drawerActiveTintColor: theme.primary,
      drawerInactiveTintColor: theme.text, 
      drawerLabelStyle: { color: theme.text, fontFamily: fonts.text }, 
      drawerStyle: { backgroundColor: theme.background },
    })}
  >
      <Drawer.Screen name="HomeAdm" component={HomeAdm} initialParams={{ usuario }} options={{ title: 'Home' }}/>
      <Drawer.Screen name="EquipesAdm" component={EquipesAdm} initialParams={{ usuario }} options={{ title: 'Equipes' }}/>
      <Drawer.Screen name="RankingAdm" component={RankingAdm} initialParams={{ usuario }} options={{ title: 'Ranking' }}/>
      <Drawer.Screen name="CadastroFuncionario" component={CadastroFuncionario} initialParams={{ usuario }} options={{ title: 'Cadastro de Funcionários' }} />
      <Drawer.Screen name="CadastroEquipes" component={CadastroEquipes} initialParams={{ usuario }} options={{ title: 'Cadastro de Equipes' }}/>
      <Drawer.Screen name="CadastroTarefas" component={CadastroTarefas} initialParams={{ usuario }} options={{ title: 'Cadastro de Tarefas' }}/>
    </Drawer.Navigator>
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
          component={DrawerAdm} 
          options={{ headerShown: false }}
        />   
        <Stack.Screen
          name="RankingEstastistico"
          component={RankingEstastistico} 
          options={{ headerShown: false }}
        />     
        <Stack.Screen
          name="EditEquipe"
          component={EditEquipe} 
          options={{ headerShown: false }}
        />           
    
      </Stack.Navigator>
    </NavigationContainer>
  );
}

